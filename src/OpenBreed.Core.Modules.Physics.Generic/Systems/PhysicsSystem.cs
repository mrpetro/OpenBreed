﻿using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Core.Modules.Physics.Messages;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Physics.Systems
{
    internal class PhysicsSystem : WorldSystem, IPhysicsSystem, IMsgHandler
    {
        #region Private Fields

        private const int CELL_SIZE = 16;

        private readonly List<DynamicPack> inactiveDynamics = new List<DynamicPack>();
        private readonly List<DynamicPack> activeDynamics = new List<DynamicPack>();
        private List<StaticPack>[] gridStatics;

        #endregion Private Fields

        #region Internal Constructors

        internal PhysicsSystem(ICore core, int gridWidth, int gridHeight) : base(core)
        {
            Require<IPhysicsComponent>();

            GridWidth = gridWidth;
            GridHeight = gridHeight;

            InitializeGrid();
        }

        #endregion Internal Constructors

        #region Public Properties

        public int GridWidth { get; }

        public int GridHeight { get; }

        #endregion Public Properties

        #region Public Methods

        public static Vector2 GetTileCenter(Position pos)
        {
            return new Vector2(pos.Value.X + CELL_SIZE / 2, pos.Value.Y + CELL_SIZE / 2);
        }

        public static Box2 GetTileBox(Position pos)
        {
            var bx = pos.Value.X;
            var by = pos.Value.Y;

            return new Box2(bx, by, bx + CELL_SIZE, by + CELL_SIZE);
        }

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.MessageBus.RegisterHandler(BodyOnMsg.TYPE, this);
            World.MessageBus.RegisterHandler(BodyOffMsg.TYPE, this);
        }

        public void Update(float dt)
        {
            SweepAndPrune(dt);
        }

        public override bool HandleMsg(object sender, IMsg msg)
        {
            switch (msg.Type)
            {
                case BodyOnMsg.TYPE:
                    return HandleBodyOnMsg(sender, (BodyOnMsg)msg);

                case BodyOffMsg.TYPE:
                    return HandleBodyOffMsg(sender, (BodyOffMsg)msg);

                default:
                    return false;
            }
        }

        public bool TryGetGridIndices(Vector2 point, out int xIndex, out int yIndex)
        {
            xIndex = (int)point.X / CELL_SIZE;
            yIndex = (int)point.Y / CELL_SIZE;
            if (xIndex < 0)
                return false;
            if (yIndex < 0)
                return false;
            if (xIndex >= GridWidth)
                return false;
            if (yIndex >= GridHeight)
                return false;

            return true;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            if (entity.Components.Any(item => item is IVelocity))
                RegisterDynamicEntity(entity);
            else
                RegisterStaticEntity(entity);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            if (entity.Components.Any(item => item is IVelocity))
                UnregisterDynamicEntity(entity);
            else
                UnregisterStaticEntity(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleBodyOnMsg(object sender, BodyOnMsg msg)
        {
            var dynamicToActivate = inactiveDynamics.FirstOrDefault(item => item.Entity == msg.Entity);

            if (dynamicToActivate != null)
            {
                activeDynamics.Add(dynamicToActivate);
                inactiveDynamics.Remove(dynamicToActivate);
                return true;
            }

            return true;
        }

        private bool HandleBodyOffMsg(object sender, BodyOffMsg msg)
        {
            var dynamicToDeactivate = activeDynamics.FirstOrDefault(item => item.Entity == msg.Entity);

            if (dynamicToDeactivate != null)
            {
                inactiveDynamics.Add(dynamicToDeactivate);
                activeDynamics.Remove(dynamicToDeactivate);
                return true;
            }

            foreach (var list in gridStatics)
            {
                var toDeactivate = list.FirstOrDefault(item => item.Entity == msg.Entity);

                if (toDeactivate != null)
                {
                    list.Remove(toDeactivate);
                    //return true;
                }
            }

            return true;
        }

        private void SweepAndPrune(float dt)
        {
            var xActiveList = new List<DynamicPack>();
            DynamicPack nextCollider = null;

            activeDynamics.Sort(Xcomparison);

            //Clear dynamics
            for (int i = 0; i < activeDynamics.Count; i++)
                activeDynamics[i].Entity.DebugData = null;

            for (int i = 0; i < activeDynamics.Count - 1; i++)
            {
                QueryStaticGrid(activeDynamics[i], dt);

                nextCollider = activeDynamics[i + 1];
                xActiveList.Add(activeDynamics[i]);

                for (int j = 0; j < xActiveList.Count; j++)
                {
                    var currentCollider = xActiveList[j];

                    if (nextCollider.Aabb.Left < currentCollider.Aabb.Right)
                    {
                        if (nextCollider.Aabb.Bottom <= currentCollider.Aabb.Top && nextCollider.Aabb.Top > currentCollider.Aabb.Bottom)
                            TestNarrowPhaseDynamic(nextCollider, currentCollider, dt);
                    }
                    else
                    {
                        xActiveList.RemoveAt(j);
                        j--;
                    }
                }
            }

            QueryStaticGrid(activeDynamics.Last(), dt);
        }

        private void TestNarrowPhaseDynamic(DynamicPack packA, DynamicPack packB, float dt)
        {
            Vector2 projection;
            if (DynamicHelper.TestVsDynamic(this, packA, packB, dt, out projection))
            {
                packA.Body.CollisionCallback?.Invoke(packB.Entity, projection);
                packB.Body.CollisionCallback?.Invoke(packA.Entity, projection);

                //bodyA.Entity.RaiseEvent(new CollisionEvent(bodyB.Entity));
                //bodyB.Entity.RaiseEvent(new CollisionEvent(bodyA.Entity));
                //DynamicHelper.ResolveVsDynamic(bodyA, bodyB, projection, dt);
            }
        }

        private void TestNarrowPhaseStatic(DynamicPack packA, StaticPack packB, float dt)
        {
            Vector2 projection;
            if (DynamicHelper.TestVsStatic(this, packA, packB, dt, out projection))
            {
                packA.Body.CollisionCallback?.Invoke(packB.Entity, projection);
                packB.Body.CollisionCallback?.Invoke(packA.Entity, projection);
            }
        }

        private void GetAabbIndices(Box2 aabb, out int left, out int right, out int bottom, out int top)
        {
            int xMod = (int)aabb.Right % CELL_SIZE;
            int yMod = (int)aabb.Top % CELL_SIZE;

            left = (int)aabb.Left >> 4;
            right = (int)aabb.Right >> 4;
            bottom = (int)aabb.Bottom >> 4;
            top = (int)aabb.Top >> 4;

            if (xMod > 0)
                right++;

            if (yMod > 0)
                top++;

            if (left < 0)
                left = 0;

            if (bottom < 0)
                bottom = 0;

            if (right > GridWidth - 1)
                right = GridWidth - 1;

            if (top > GridHeight - 1)
                top = GridHeight - 1;
        }

        private void QueryStaticGrid(DynamicPack pack, float dt)
        {
            var dynamicAabb = pack.Aabb;

            pack.Body.Boxes = new List<Tuple<int, int>>();

            int leftIndex;
            int rightIndex;
            int bottomIndex;
            int topIndex;

            GetAabbIndices(dynamicAabb, out leftIndex, out rightIndex, out bottomIndex, out topIndex);

            //Collect all unique aabb boxes
            var boxesSet = new List<StaticPack>();
            for (int yIndex = bottomIndex; yIndex < topIndex; yIndex++)
            {
                for (int xIndex = leftIndex; xIndex < rightIndex; xIndex++)
                {
                    pack.Body.Boxes.Add(new Tuple<int, int>(xIndex, yIndex));
                    var collideres = gridStatics[xIndex + GridWidth * yIndex];
                    for (int boxIndex = 0; boxIndex < collideres.Count; boxIndex++)
                    {
                        if(!boxesSet.Contains(collideres[boxIndex]))
                            boxesSet.Add(collideres[boxIndex]);
                    }
                }
            }

            if (boxesSet.Count == 0)
                return;

            var pos = dynamicAabb.GetCenter();

            boxesSet.Sort((a, b) => ShortestDistanceComparer(pos, GetTileCenter(a.Position), GetTileCenter(b.Position)));

            //Iterate all collected static bodies for detail test
            foreach (var item in boxesSet)
                TestNarrowPhaseStatic(pack, item, dt);
        }

        private int Xcomparison(DynamicPack x, DynamicPack y)
        {
            var xAabb = x.Aabb;
            var yAabb = y.Aabb;

            if (xAabb.Left < yAabb.Left)
                return -1;
            if (xAabb.Left == yAabb.Left)
                return 0;
            else
                return 1;
        }

        private int ShortestDistanceComparer(Vector2 pos, Vector2 a, Vector2 b)
        {
            var lx = Vector2.Distance(pos, a);
            var ly = Vector2.Distance(pos, b);

            if (lx < ly)
                return -1;
            if (lx == ly)
                return 0;
            else
                return 1;
        }

        private void RegisterStaticEntity(IEntity entity)
        {
            var pack = new StaticPack(entity,
                                      entity.Components.OfType<IBody>().First(),
                                      entity.Components.OfType<Position>().First(),
                                      entity.Components.OfType<IShapeComponent>().First());

            var pos = pack.Position;

            int xIndex;
            int yIndex;

            if (!TryGetGridIndices(pos.Value, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            var aabb = pack.Aabb;

            int leftIndex;
            int rightIndex;
            int bottomIndex;
            int topIndex;

            GetAabbIndices(aabb, out leftIndex, out rightIndex, out bottomIndex, out topIndex);

            for (int j = bottomIndex; j < topIndex; j++)
            {
                var gridIndex = GridWidth * j + leftIndex;
                for (int i = leftIndex; i < rightIndex; i++)
                {
                    gridStatics[gridIndex].Add(pack);
                    gridIndex++;
                }
            }
        }


        private void RegisterDynamicEntity(IEntity entity)
        {
            var pack = new DynamicPack(entity,
                                      entity.Components.OfType<IBody>().First(),
                                      entity.Components.OfType<IPosition>().First(),
                                      entity.Components.OfType<IVelocity>().First(),
                                      entity.Components.OfType<IShapeComponent>().First());

            activeDynamics.Add(pack);
        }

        private void UnregisterDynamicEntity(IEntity entity)
        {
            var dynamic = activeDynamics.FirstOrDefault(item => item.Entity == entity);

            if (dynamic == null)
                throw new InvalidOperationException("Entity not found in this system.");

            activeDynamics.Remove(dynamic);
        }

        private void UnregisterStaticEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        private void InitializeGrid()
        {
            gridStatics = new List<StaticPack>[GridWidth * GridHeight];

            for (int i = 0; i < gridStatics.Length; i++)
                gridStatics[i] = new List<StaticPack>();
        }

        #endregion Private Methods
    }
}
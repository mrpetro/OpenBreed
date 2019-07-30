using OpenBreed.Core.Common;
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

        #endregion Public Methods

        #region Protected Methods

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
                    return true;
                }
            }

            return true;
        }


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

        private void TestNarrowPhaseDynamic(DynamicPack bodyA, DynamicPack bodyB, float dt)
        {
            Vector2 projection;
            if (DynamicHelper.TestVsDynamic(this, bodyA, bodyB, dt, out projection))
            {
                bodyA.Entity.RaiseEvent(new CollisionEvent(bodyB.Entity));
                bodyB.Entity.RaiseEvent(new CollisionEvent(bodyA.Entity));
                DynamicHelper.ResolveVsDynamic(bodyA, bodyB, projection, dt);
            }
        }

        private void TestNarrowPhaseStatic(DynamicPack bodyA, StaticPack bodyB, float dt)
        {
            Vector2 projection;
            if (DynamicHelper.TestVsStatic(this, bodyA, bodyB, dt, out projection))
            {
                bodyA.Entity.RaiseEvent(new CollisionEvent(bodyB.Entity));
                bodyB.Entity.RaiseEvent(new CollisionEvent(bodyA.Entity));
                DynamicHelper.ResolveVsStatic(bodyA, bodyB, projection, dt);
            }
        }

        private void QueryStaticGrid(DynamicPack pack, float dt)
        {
            var dynamicAabb = pack.Aabb;

            pack.Body.Boxes = new List<Tuple<int, int>>();

            int xMod = (int)dynamicAabb.Right % CELL_SIZE;
            int yMod = (int)dynamicAabb.Top % CELL_SIZE;

            int leftIndex = (int)dynamicAabb.Left >> 4;
            int rightIndex = (int)dynamicAabb.Right >> 4;
            int bottomIndex = (int)dynamicAabb.Bottom >> 4;
            int topIndex = (int)dynamicAabb.Top >> 4;

            if (xMod > 0)
                rightIndex++;

            if (yMod > 0)
                topIndex++;

            if (leftIndex < 0)
                leftIndex = 0;

            if (bottomIndex < 0)
                bottomIndex = 0;

            if (rightIndex > GridWidth - 1)
                rightIndex = GridWidth - 1;

            if (topIndex > GridHeight - 1)
                topIndex = GridHeight - 1;

            //Collect all unique aabb boxes
            var boxesSet = new List<StaticPack>();
            for (int yIndex = bottomIndex; yIndex < topIndex; yIndex++)
            {
                for (int xIndex = leftIndex; xIndex < rightIndex; xIndex++)
                {
                    pack.Body.Boxes.Add(new Tuple<int, int>(xIndex, yIndex));
                    var collideres = gridStatics[xIndex + GridWidth * yIndex];
                    for (int boxIndex = 0; boxIndex < collideres.Count; boxIndex++)
                        boxesSet.Add(collideres[boxIndex]);
                }
            }

            if (boxesSet.Count == 0)
                return;

            var pos = dynamicAabb.GetCenter();

            boxesSet.Sort((x, y) => ShortestDistanceComparer(pos, x.Aabb, y.Aabb));

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

        private int ShortestDistanceComparer(Vector2 pos, Box2 x, Box2 y)
        {
            var lx = Vector2.Distance(pos, x.GetCenter());
            var ly = Vector2.Distance(pos, y.GetCenter());

            if (lx < ly)
                return -1;
            if (lx == ly)
                return 0;
            else
                return 1;
        }

        private void GetGridIndices(Vector2 pos, out int x, out int y)
        {
            x = (int)(pos.X / CELL_SIZE);
            y = (int)(pos.Y / CELL_SIZE);
        }

        private void RegisterStaticEntity(IEntity entity)
        {
            var pack = new StaticPack(entity,
                                      entity.Components.OfType<IBody>().First(),
                                      entity.Components.OfType<IPosition>().First(),
                                      entity.Components.OfType<IShapeComponent>().First());

            var position = pack.Position;

            int x, y;
            GetGridIndices(position.Value, out x, out y);

            var gridIndex = x + GridHeight * y;

            if (x >= GridWidth)
                throw new InvalidOperationException($"Grid box body X coordinate exceeds grid width size.");

            if (y >= GridHeight)
                throw new InvalidOperationException($"Grid box body Y coordinate exceeds grid height size.");

            gridStatics[gridIndex].Add(pack);
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
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Physics.Systems
{
    internal class PhysicsSystem : WorldSystem, IPhysicsSystem
    {
        #region Private Fields

        private const int CELL_SIZE = 16;

        private static readonly AabbXComparer comparer = new AabbXComparer();

        private readonly List<DynamicPack> dynamicPacks = new List<DynamicPack>();

        private List<IEntity>[] gridStaticEntities;
        private List<IStaticBody>[] gridStaticComps;

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

        public void Update(float dt)
        {
            BruteForce(dt);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            var physicsComponent = entity.Components.OfType<IPhysicsComponent>().First();

            if (physicsComponent is IStaticBody)
                RegisterStaticEntity(entity, (IStaticBody)physicsComponent);
            else if (physicsComponent is IDynamicBody)
                RegisterDynamicEntity(entity, (IDynamicBody)physicsComponent);
            else
                throw new NotImplementedException($"{physicsComponent}");
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var physicsComponent = entity.Components.OfType<IPhysicsComponent>().First();

            if (physicsComponent is IStaticBody)
                UnregisterStaticEntity(entity);
            else if (physicsComponent is IDynamicBody)
                UnregisterDynamicEntity(entity);
            else
                throw new NotImplementedException($"{physicsComponent}");
        }

        #endregion Protected Methods

        #region Private Methods

        private void UnregisterDynamicEntity(IEntity entity)
        {
            var dynamic = dynamicPacks.FirstOrDefault(item => item.Entity == entity);

            if (dynamic == null)
                throw new InvalidOperationException("Entity not found in this system.");

            dynamicPacks.Remove(dynamic);
        }

        private void UnregisterStaticEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        private void BruteForce(float dt)
        {
            var xActiveList = new List<DynamicPack>();
            DynamicPack nextCollider = null;

            dynamicPacks.Sort(Xcomparison);

            //Clear dynamics
            for (int i = 0; i < dynamicPacks.Count; i++)
                dynamicPacks[i].Entity.DebugData = null;

            for (int i = 0; i < dynamicPacks.Count - 1; i++)
            {
                //for (int e = i; e < dynamicBodyComps.Count; e++)
                //{
                //    if (CheckBoundingBoxForOverlap(i, e))
                //        DetectNarrowPhase(i, e);
                //}


                QueryStaticMatrix(dynamicPacks[i], dt);


                nextCollider = dynamicPacks[i + 1];
                xActiveList.Add(dynamicPacks[i]);

                for (int j = 0; j < xActiveList.Count; j++)
                {
                    var currentCollider = xActiveList[j];

                    if (nextCollider.Aabb.Left < currentCollider.Aabb.Right)
                    {
                        if (nextCollider.Aabb.Bottom <= currentCollider.Aabb.Top && nextCollider.Aabb.Top > currentCollider.Aabb.Bottom)
                            DynamicHelper.CollideVsDynamic(nextCollider, currentCollider);
                    }
                    else
                    {
                        xActiveList.RemoveAt(j);

                        j--;
                    }
                }
            }

            QueryStaticMatrix(dynamicPacks.Last(), dt);
        }

        private void QueryStaticMatrix(DynamicPack pack, float dt)
        {
            var dynamicAabb = pack.Aabb;

            pack.Body.Collides = false;
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
            var boxesSet = new List<IStaticBody>();
            for (int yIndex = bottomIndex; yIndex < topIndex; yIndex++)
            {
                for (int xIndex = leftIndex; xIndex < rightIndex; xIndex++)
                {
                    pack.Body.Boxes.Add(new Tuple<int, int>(xIndex, yIndex));
                    var collideres = gridStaticComps[xIndex + GridWidth * yIndex];
                    for (int boxIndex = 0; boxIndex < collideres.Count; boxIndex++)
                    {
                        boxesSet.Add(collideres[boxIndex]);
                    }
                }
            }

            if (boxesSet.Count == 0)
                return;

            var pos = dynamicAabb.GetCenter();

            boxesSet.Sort((x, y) => ShortestDistanceComparer(pos, x, y));

            //Iterate all collected static bodies for detail test
            foreach (var item in boxesSet)
            {
                DynamicHelper.CollideVsStatic(pack, item, dt);
            }
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

        private int ShortestDistanceComparer(Vector2 pos, IStaticBody x, IStaticBody y)
        {
            var lx = Vector2.Distance(pos, x.Aabb.GetCenter());
            var ly = Vector2.Distance(pos, y.Aabb.GetCenter());

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

        private void RegisterStaticEntity(IEntity entity, IStaticBody body)
        {
            var position = entity.Components.OfType<IPosition>().First();

            body.Aabb = new Box2
            {
                Left = position.Value.X,
                Bottom = position.Value.Y,
                Right = position.Value.X + CELL_SIZE,
                Top = position.Value.Y + CELL_SIZE,
            };

            int x, y;
            GetGridIndices(position.Value, out x, out y);

            var gridIndex = x + GridHeight * y;

            if (x >= GridWidth)
                throw new InvalidOperationException($"Grid box body X coordinate exceeds grid width size.");

            if (y >= GridHeight)
                throw new InvalidOperationException($"Grid box body Y coordinate exceeds grid height size.");

            gridStaticEntities[gridIndex].Add(entity);
            gridStaticComps[gridIndex].Add(body);
        }

        private void RegisterDynamicEntity(IEntity entity, IDynamicBody body)
        {
            var pack = new DynamicPack(entity, 
                                      body,
                                      entity.Components.OfType<IPosition>().First(),
                                      entity.Components.OfType<IVelocity>().First(),
                                      entity.Components.OfType<IShapeComponent>().First());

            dynamicPacks.Add(pack);
        }

        private void InitializeGrid()
        {
            gridStaticEntities = new List<IEntity>[GridWidth * GridHeight];
            gridStaticComps = new List<IStaticBody>[GridWidth * GridHeight];

            for (int i = 0; i < gridStaticEntities.Length; i++)
                gridStaticEntities[i] = new List<IEntity>();

            for (int i = 0; i < gridStaticComps.Length; i++)
                gridStaticComps[i] = new List<IStaticBody>();
        }

        #endregion Private Methods
    }
}
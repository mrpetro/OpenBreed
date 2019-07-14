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

        private static readonly AabbXComparer comparer = new AabbXComparer();

        private readonly List<IEntity> dynamicEntities = new List<IEntity>();
        private readonly List<IDynamicBody> dynamicBodyComps = new List<IDynamicBody>();
        private readonly List<IPosition> positionComps = new List<IPosition>();
        private readonly List<IVelocity> velocityComps = new List<IVelocity>();
        private readonly List<IShapeComponent> shapeComps = new List<IShapeComponent>();

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

            physicsComponent.Initialize(entity);

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

            physicsComponent.Initialize(entity);

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
            var index = dynamicEntities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            dynamicEntities.RemoveAt(index);
            positionComps.RemoveAt(index);
            velocityComps.RemoveAt(index);
            shapeComps.RemoveAt(index);
        }

        private void UnregisterStaticEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        private void BruteForce(float dt)
        {
            List<IDynamicBody> activeList = new List<IDynamicBody>();

            for (int i = 0; i < dynamicBodyComps.Count; i++)
            {
                //for (int e = i; e < dynamicBodies.Count; e++)
                //{
                //    if (CheckBoundingBoxForOverlap(i, e))
                //        DetectNarrowPhase(i, e);
                //}

                QueryStaticMatrix(i,dt);
            }
        }

        private void QueryStaticMatrix(int index, float dt)
        {
            var entity = dynamicEntities[index];
            var dynamicBody = dynamicBodyComps[index];
            var position = positionComps[index];
            var velocity = velocityComps[index];
            var shape = shapeComps[index];

            var dynamicAabb = shape.Aabb.Translated(position.Value);

            dynamicBody.Collides = false;
            dynamicBody.Boxes = new List<Tuple<int, int>>();

            int xMod = (int)dynamicAabb.Right % 16;
            int yMod = (int)dynamicAabb.Top % 16;

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
                    dynamicBody.Boxes.Add(new Tuple<int, int>(xIndex, yIndex));
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
                dynamicAabb = shape.Aabb.Translated(position.Value);
                DynamicHelper.CollideDynamicVsStatic(dynamicBody, position, velocity, dynamicAabb, item, dt);
            }
        }

        //public override void Update(float dt)
        //{
        //    dynamicBodies.Sort(Xcomparison);

        //    List<IDynamicBody> activeList = new List<IDynamicBody>();
        //    IDynamicBody next_collider = null;

        //    for (int i = 0; i < dynamicBodies.Count - 1; i++)
        //    {
        //        //QueryStaticMatrix(m_Dynamics[i]);

        //        next_collider = dynamicBodies[i + 1];
        //        activeList.Add(dynamicBodies[i]);

        //        foreach (var item in activeList)
        //        {
        //            if (next_collider.Aabb.Left < item.Aabb.Right)
        //            {
        //                if (next_collider.Aabb.Bottom <= item.Aabb.Top && next_collider.Aabb.Top > item.Aabb.Bottom)
        //                {
        //                    next_collider->CollideWith(item);
        //                }

        //                continue;
        //                lst_iter++;
        //            }
        //            else
        //            {
        //                lst_iter = activeList.Remove(item);
        //            }
        //        }
        //    }
        private int Xcomparison(IDynamicBody x, IDynamicBody y)
        {
            if (x.Aabb.Left < y.Aabb.Left)
                return -1;
            if (x.Aabb.Left == y.Aabb.Left)
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

        private void RegisterStaticEntity(IEntity entity, IStaticBody body)
        {
            int x, y;
            body.GetGridIndices(out x, out y);
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
            dynamicEntities.Add(entity);
            dynamicBodyComps.Add(body);
            positionComps.Add(entity.Components.OfType<IPosition>().First());
            velocityComps.Add(entity.Components.OfType<IVelocity>().First());

            shapeComps.Add(entity.Components.OfType<IShapeComponent>().First());
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
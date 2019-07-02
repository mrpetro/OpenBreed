﻿using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Physics.Systems
{
    public class PhysicsSystem : WorldSystem, IPhysicsSystem
    {
        #region Private Fields

        private static readonly AabbXComparer comparer = new AabbXComparer();

        private readonly List<IEntity> dynamicEntities = new List<IEntity>();
        private readonly List<IDynamicBody> dynamicBodies = new List<IDynamicBody>();
        private List<IEntity>[] gridStaticEntities;
        private List<IStaticBody>[] gridStaticComps;

        #endregion Private Fields

        #region Public Constructors

        public PhysicsSystem(ICore core, int gridWidth, int gridHeight) : base(core)
        {
            Require<IPhysicsComponent>();

            GridWidth = gridWidth;
            GridHeight = gridHeight;

            InitializeGrid();

            dynamicEntities = new List<IEntity>();
            dynamicBodies = new List<IDynamicBody>();
        }

        #endregion Public Constructors

        #region Public Properties

        public int GridWidth { get; }

        public int GridHeight { get; }

        #endregion Public Properties

        #region Public Methods

        public void Update(float dt)
        {
            Integrate();

            BruteForce(dt);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            var physicsComponent = entity.Components.OfType<IPhysicsComponent>().First();

            physicsComponent.Initialize(entity);

            if (physicsComponent is IStaticBody)
                AddStaticBody(entity, (IStaticBody)physicsComponent);
            else if (physicsComponent is IDynamicBody)
                AddDynamicBody(entity, (IDynamicBody)physicsComponent);
            else
                throw new NotImplementedException($"{physicsComponent}");
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        private void BruteForce(float dt)
        {
            List<IDynamicBody> activeList = new List<IDynamicBody>();

            for (int i = 0; i < dynamicBodies.Count; i++)
            {
                //for (int e = i; e < dynamicBodies.Count; e++)
                //{
                //    if (CheckBoundingBoxForOverlap(i, e))
                //        DetectNarrowPhase(i, e);
                //}

                QueryStaticMatrix(dynamicBodies[i]);
            }
        }

        private void QueryStaticMatrix(IDynamicBody collider)
        {
            collider.Collides = false;
            collider.Boxes = new List<Tuple<int, int>>();

            int xMod = (int)collider.Aabb.Right % 16;
            int yMod = (int)collider.Aabb.Top % 16;

            int leftIndex = (int)collider.Aabb.Left >> 4;
            int rightIndex = (int)collider.Aabb.Right >> 4;
            int bottomIndex = (int)collider.Aabb.Bottom >> 4;
            int topIndex = (int)collider.Aabb.Top >> 4;

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
            var boxesSet = new HashSet<IStaticBody>();
            for (int yIndex = bottomIndex; yIndex < topIndex; yIndex++)
            {
                for (int xIndex = leftIndex; xIndex < rightIndex; xIndex++)
                {
                    collider.Boxes.Add(new Tuple<int, int>(xIndex, yIndex));
                    var collideres = gridStaticComps[xIndex + GridWidth * yIndex];
                    for (int boxIndex = 0; boxIndex < collideres.Count; boxIndex++)
                    {
                        boxesSet.Add(collideres[boxIndex]);
                    }
                }
            }

            if (boxesSet.Count == 0)
                return;

            //Iterate all collected static bodies for detail test
            foreach (var item in boxesSet)
            {
                collider.CollideVsStatic(item);
            }
        }

        private void CollideDynamicVsStatic(IDynamicBody dynamicBody, IStaticBody staticBody)
        {
        }

        private void Integrate()
        {
            for (int i = 0; i < dynamicBodies.Count; i++)
                dynamicBodies[i].IntegrateVerlet();
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

        private void AddStaticBody(IEntity entity, IStaticBody body)
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

        private void AddDynamicBody(IEntity entity, IDynamicBody body)
        {
            dynamicEntities.Add(entity);
            dynamicBodies.Add(body);
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
using OpenBreed.Game.Common;
using OpenBreed.Game.Physics.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Game.Physics
{
    public class PhysicsSystem : WorldSystem<IPhysicsComponent>
    {
        #region Private Fields

        private IStaticBody[] gridArray;
        private List<IDynamicBody> dynamicBodies;

        #endregion Private Fields

        #region Public Constructors

        public PhysicsSystem(int gridWidth, int gridHeight)
        {
            GridWidth = gridWidth;
            GridHeight = gridHeight;

            InitializeGrid();

            dynamicBodies = new List<IDynamicBody>();
        }

        #endregion Public Constructors

        #region Public Properties

        public int GridWidth { get; }

        public int GridHeight { get; }

        #endregion Public Properties

        #region Protected Methods

        protected override void AddComponent(IPhysicsComponent component)
        {
            if (component is IStaticBody)
                AddStaticBody((IStaticBody)component);
            else if (component is IDynamicBody)
                AddDynamicBody((IDynamicBody)component);
            else
                throw new NotImplementedException($"{component}");
        }

        protected override void RemoveComponent(IPhysicsComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        public override void Update(float dt)
        {


            //sort(m_Dynamics.begin(), m_Dynamics.end(), CollisionSystem::AABBCompareX);


            //List<IDynamicBody> activeList = new List<IDynamicBody>();
            //IDynamicBody next_collider = null;

            //for (int i = 0; i < dynamicBodies.Count - 1; i++)
            //{
            //    //QueryStaticMatrix(m_Dynamics[i]);

            //    next_collider = dynamicBodies[i + 1];
            //    activeList.Add(dynamicBodies[i]);

            //    foreach (var item in activeList)
            //    {
            //        ICollisionComponent* curr_collider = *lst_iter;

            //        if (next_collider->GetLeft() < curr_collider->GetRight())
            //        {
            //            if (next_collider->GetDown() <= curr_collider->GetUp() && next_collider->GetUp() > curr_collider->GetDown())
            //            {
            //                next_collider->CollideWith(curr_collider);
            //            }

            //            lst_iter++;
            //        }
            //        else
            //        {
            //            lst_iter = activeList.Remove(item);
            //        }
            //    }


            //    list<ICollisionComponent*>::iterator lst_iter = activeList.begin();
            //    while (lst_iter != activeList.end())
            //    {
            //        ICollisionComponent* curr_collider = *lst_iter;

            //        if (next_collider->GetLeft() < curr_collider->GetRight())
            //        {
            //            if (next_collider->GetDown() <= curr_collider->GetUp() && next_collider->GetUp() > curr_collider->GetDown())
            //            {
            //                next_collider->CollideWith(curr_collider);
            //            }

            //            lst_iter++;
            //        }
            //        else
            //        {
            //            lst_iter = activeList.erase(lst_iter);
            //        }
            //    }
            //}

            //QueryStaticMatrix(m_Dynamics.back());

        }

        private void AddStaticBody(IStaticBody body)
        {
            int x, y;
            body.GetGridIndices(out x, out y);
            var gridIndex = x + GridHeight * y;

            if (x >= GridWidth)
                throw new InvalidOperationException($"Grid box body X coordinate exceeds grid width size.");

            if (y >= GridHeight)
                throw new InvalidOperationException($"Grid box body Y coordinate exceeds grid height size.");

            gridArray[gridIndex] = body;
        }

        private void AddDynamicBody(IDynamicBody body)
        {
            dynamicBodies.Add(body);
        }

        private void InitializeGrid()
        {
            gridArray = new IStaticBody[GridWidth * GridHeight];
        }

        #endregion Private Methods
    }
}
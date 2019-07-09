//using OpenBreed.Core.Modules.Animation.Systems.Common;
//using OpenBreed.Core.Modules.Physics.Components;
//using OpenBreed.Core.Modules.Physics.Helpers;
//using System;
//using System.Collections.Generic;
//using OpenBreed.Core.Modules.Animation.Systems;

//namespace OpenBreed.Core.Modules.Physics.Systems
//{
//    public class DynamicPhysicsSystem : WorldSystemEx, IUpdatableSystemEx
//    {
//        #region Private Fields

//        private static readonly AabbXComparer comparer = new AabbXComparer();

//        private List<IDynamicBody> dynamicBodies;

//        #endregion Private Fields

//        #region Public Constructors

//        public DynamicPhysicsSystem(ICore core, int gridWidth, int gridHeight) : base(core)
//        {
//            GridWidth = gridWidth;
//            GridHeight = gridHeight;

//            InitializeGrid();

//            dynamicBodies = new List<IDynamicBody>();
//        }

//        #endregion Public Constructors

//        #region Public Properties

//        public int GridWidth { get; }

//        public int GridHeight { get; }

//        #endregion Public Properties

//        #region Public Methods

//        public void Update(float dt)
//        {
//            Integrate();

//            BruteForce(dt);
//        }

//        #endregion Public Methods

//        #region Protected Methods

//        #endregion Protected Methods

//        #region Private Methods

//        private void BruteForce(float dt)
//        {
//            List<IDynamicBody> activeList = new List<IDynamicBody>();

//            for (int i = 0; i < dynamicBodies.Count; i++)
//            {
//                //for (int e = i; e < dynamicBodies.Count; e++)
//                //{
//                //    if (CheckBoundingBoxForOverlap(i, e))
//                //        DetectNarrowPhase(i, e);
//                //}

//                QueryStaticMatrix(dynamicBodies[i]);
//            }
//        }

//        private void QueryStaticMatrix(IDynamicBody collider)
//        {
//            collider.Collides = false;
//            collider.Boxes = new List<Tuple<int, int>>();

//            int xMod = (int)collider.Aabb.Right % 16;
//            int yMod = (int)collider.Aabb.Top % 16;

//            int leftIndex = (int)collider.Aabb.Left >> 4;
//            int rightIndex = (int)collider.Aabb.Right >> 4;
//            int bottomIndex = (int)collider.Aabb.Bottom >> 4;
//            int topIndex = (int)collider.Aabb.Top >> 4;

//            if (xMod > 0)
//                rightIndex++;

//            if (yMod > 0)
//                topIndex++;

//            if (leftIndex < 0)
//                leftIndex = 0;

//            if (bottomIndex < 0)
//                bottomIndex = 0;

//            if (rightIndex > GridWidth - 1)
//                rightIndex = GridWidth - 1;

//            if (topIndex > GridHeight - 1)
//                topIndex = GridHeight - 1;

//            //Collect all unique aabb boxes
//            var boxesSet = new HashSet<IStaticBody>();
//            for (int yIndex = bottomIndex; yIndex < topIndex; yIndex++)
//            {
//                for (int xIndex = leftIndex; xIndex < rightIndex; xIndex++)
//                {
//                    collider.Boxes.Add(new Tuple<int, int>(xIndex, yIndex));
//                    var collideres = gridArray[xIndex + GridWidth * yIndex];
//                    for (int boxIndex = 0; boxIndex < collideres.Count; boxIndex++)
//                    {
//                        boxesSet.Add(collideres[boxIndex]);
//                    }
//                }
//            }

//            if (boxesSet.Count == 0)
//                return;

//            //Iterate all collected static bodies for detail test
//            foreach (var item in boxesSet)
//            {
//                collider.CollideVsStatic(item);
//            }
//        }

//        private void CollideDynamicVsStatic(IDynamicBody dynamicBody, IStaticBody staticBody)
//        {
//        }

//        private void Integrate()
//        {
//            for (int i = 0; i < dynamicBodies.Count; i++)
//                dynamicBodies[i].IntegrateVerlet();
//        }

//        //public override void Update(float dt)
//        //{
//        //    dynamicBodies.Sort(Xcomparison);

//        //    List<IDynamicBody> activeList = new List<IDynamicBody>();
//        //    IDynamicBody next_collider = null;

//        //    for (int i = 0; i < dynamicBodies.Count - 1; i++)
//        //    {
//        //        //QueryStaticMatrix(m_Dynamics[i]);

//        //        next_collider = dynamicBodies[i + 1];
//        //        activeList.Add(dynamicBodies[i]);

//        //        foreach (var item in activeList)
//        //        {
//        //            if (next_collider.Aabb.Left < item.Aabb.Right)
//        //            {
//        //                if (next_collider.Aabb.Bottom <= item.Aabb.Top && next_collider.Aabb.Top > item.Aabb.Bottom)
//        //                {
//        //                    next_collider->CollideWith(item);
//        //                }

//        //                continue;
//        //                lst_iter++;
//        //            }
//        //            else
//        //            {
//        //                lst_iter = activeList.Remove(item);
//        //            }
//        //        }
//        //    }
//        private int Xcomparison(IDynamicBody x, IDynamicBody y)
//        {
//            if (x.Aabb.Left < y.Aabb.Left)
//                return -1;
//            if (x.Aabb.Left == y.Aabb.Left)
//                return 0;
//            else
//                return 1;
//        }

//        private void AddStaticBody(IStaticBody body)
//        {
//            int x, y;
//            body.GetGridIndices(out x, out y);
//            var gridIndex = x + GridHeight * y;

//            if (x >= GridWidth)
//                throw new InvalidOperationException($"Grid box body X coordinate exceeds grid width size.");

//            if (y >= GridHeight)
//                throw new InvalidOperationException($"Grid box body Y coordinate exceeds grid height size.");

//            gridArray[gridIndex].Add(body);
//        }

//        private void AddDynamicBody(IDynamicBody body)
//        {
//            dynamicBodies.Add(body);
//        }

//        private void InitializeGrid()
//        {
//            gridArray = new List<IStaticBody>[GridWidth * GridHeight];

//            for (int i = 0; i < gridArray.Length; i++)
//                gridArray[i] = new List<IStaticBody>();
//        }

//        #endregion Private Methods
//    }
//}
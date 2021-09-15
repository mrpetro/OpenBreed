using OpenBreed.Common.Logging;
using OpenBreed.Physics.Interface.Managers;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Physics.Generic.Managers
{
    public class BroadphaseMan : IBroadphaseMan
    {
        #region Private Fields

        private readonly ILogger logger;
        private readonly List<BroadphaseGrid> items = new List<BroadphaseGrid>();

        #endregion Private Fields

        #region Internal Constructors

        internal BroadphaseMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public int CreateGrid(int width, int height, int cellSize)
        {
            var broadphaseGrid = new BroadphaseGrid(width, height, cellSize);
            return Register(broadphaseGrid);
        }

        public void InsertStatic(int gridId, int entityId, Box2 aabb)
        {
            var grid = items[gridId];

            grid.InsertStatic(entityId, aabb);
        }

        public void RemoveStatic(int gridId, int entityId, Box2 aabb)
        {
            var grid = items[gridId];

            grid.RemoveStatic(entityId, aabb);
        }

        public HashSet<int> QueryStatic(int gridId, Box2 aabb)
        {
            var grid = items[gridId];

            return grid.QueryStatic(aabb);
        }

        #endregion Public Methods

        #region Internal Methods

        internal int Register(BroadphaseGrid broadphaseGridGrid)
        {
            items.Add(broadphaseGridGrid);

            logger.Verbose($"Broadphase grid with ID '{items.Count - 1}' created.");

            return items.Count - 1;
        }




        #endregion Internal Methods
    }
}
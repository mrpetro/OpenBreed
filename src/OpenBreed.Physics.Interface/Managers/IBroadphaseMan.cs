using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Interface.Managers
{
    public interface IBroadphaseMan
    {
        /// <summary>
        /// Create a grid giving it's properties and return it's ID
        /// </summary>
        /// <param name="width">Width of grid</param>
        /// <param name="height">Height of grid</param>
        /// <param name="cellSize">Grid cell size</param>
        /// <returns></returns>
        int CreateGrid(int width, int height, int cellSize);

        void InsertStatic(int gridId, int entityId, Box2 aabb);

        void RemoveStatic(int gridId, int id, Box2 aabb);

        HashSet<int> QueryStatic(int gridId, Box2 aabb);
    }
}

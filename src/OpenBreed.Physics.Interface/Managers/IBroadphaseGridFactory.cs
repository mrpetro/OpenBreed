using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Interface.Managers
{
    /// <summary>
    /// Factory for creating collision detection broadphase grid
    /// </summary>
    public interface IBroadphaseGridFactory
    {
        /// <summary>
        /// Create a grid giving it's properties and return it's ID
        /// </summary>
        /// <param name="width">Width of grid</param>
        /// <param name="height">Height of grid</param>
        /// <param name="cellSize">Grid cell size</param>
        /// <returns></returns>
        IBroadphaseGrid CreateGrid(int width, int height, int cellSize);
    }
}

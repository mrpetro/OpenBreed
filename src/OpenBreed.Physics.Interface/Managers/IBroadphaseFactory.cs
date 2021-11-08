using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Interface.Managers
{
    /// <summary>
    /// Factory for creating collision detection broadphase
    /// </summary>
    public interface IBroadphaseFactory
    {
        /// <summary>
        /// Create a static broadphase grid giving it's properties and return it's ID
        /// </summary>
        /// <param name="width">Width of grid</param>
        /// <param name="height">Height of grid</param>
        /// <param name="cellSize">Grid cell size</param>
        /// <returns></returns>
        IBroadphaseStatic CreateStatic(int width, int height, int cellSize);

        /// <summary>
        /// Create dynamic broadphase
        /// </summary>
        /// <returns></returns>
        IBroadphaseDynamic CreateDynamic();
    }
}

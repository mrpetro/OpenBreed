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
        /// Create broadphase
        /// </summary>
        /// <param name="width">Width of static grid</param>
        /// <param name="height">Height of static grid</param>
        /// <param name="cellSize">Static grid cell size</param>
        /// <returns></returns>
        IBroadphase CreateDynamic(int width, int height, int cellSize);
    }
}

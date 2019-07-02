using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems
{
    /// <summary>
    /// System that state will be updated during core update phase
    /// </summary>
    public interface IUpdatableSystem : IWorldSystem
    {
        /// <summary>
        /// Update this system with given time step
        /// </summary>
        /// <param name="dt">Time step</param>
        void Update(float dt);
    }
}

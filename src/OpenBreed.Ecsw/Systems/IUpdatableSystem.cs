using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Ecsw.Systems
{
    /// <summary>
    /// System that state will be updated during core update phase
    /// </summary>
    public interface IUpdatableSystem : IWorldSystem
    {
        /// <summary>
        /// Update all entities in this system using given time step
        /// </summary>
        /// <param name="dt">Time step</param>
        void Update(float dt);

        /// <summary>
        /// Update only enitites immune from pausing in this system using given time step
        /// </summary>
        /// <param name="dt">Time step</param>
        void UpdatePauseImmuneOnly(float dt);
    }
}

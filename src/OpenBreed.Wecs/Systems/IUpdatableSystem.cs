using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems
{
    /// <summary>
    /// System that state will be updated during core update phase
    /// </summary>
    public interface IUpdatableSystem : IMatchingSystem
    {
        /// <summary>
        /// Update all entities in this system using given time step
        /// </summary>
        /// <param name="context">World context</param>
        void Update(IUpdateContext context);
    }
}

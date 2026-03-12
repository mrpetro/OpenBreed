using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Abstractions.Systems
{
    /// <summary>
    /// System that updates given entities.
    /// </summary>
    public interface IUpdatableSystem : ISystem
    {
        /// <summary>
        /// Update all entities in this system using given update context.
        /// </summary>
        /// <param name="entities">Entities to update.</param>
        /// <param name="context">World context.</param>
        void Update(IEnumerable<IEntity> entities, IUpdateContext context);
    }
}

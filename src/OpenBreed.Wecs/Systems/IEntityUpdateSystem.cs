using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
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
    public interface IEntityUpdateSystem : ISystem
    {
        /// <summary>
        /// Update entity using given world context
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <param name="context">World context</param>
        void Update(IEntity entity, IWorldContext context);
    }
}

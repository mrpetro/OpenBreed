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
    /// Interface of subsystem that updates single entity
    /// </summary>
    public interface IEntityUpdateSubsystem : ISubsystem
    {
        /// <summary>
        /// Update entity using given world context
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <param name="context">World context</param>
        void Update(IEntity entity, IWorldContext context);
    }
}

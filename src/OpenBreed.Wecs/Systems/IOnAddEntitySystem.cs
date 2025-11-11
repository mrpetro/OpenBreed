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
    /// System that updates when entity is added to world.
    /// </summary>
    public interface IOnAddEntitySystem
    {
        #region Public Methods

        /// <summary>
        /// Method that is called when entity is added to world
        /// </summary>
        /// <param name="world">World of entity that is being added to it.</param>
        /// <param name="entity">Entity that is being added.</param>
        void OnAddEntity(IWorld world, IEntity entity);

        #endregion Public Methods
    }
}
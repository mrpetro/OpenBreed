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
    /// System that updates when entity is removed from world. 
    /// </summary>
    public interface IOnRemoveEntitySystem
    {
        #region Public Methods

        /// <summary>
        /// Method that is called when entity is removed from world
        /// </summary>
        /// <param name="world">World of entity that is being removed from it.</param>
        /// <param name="entity">Entity that is being removed.</param>
        void OnRemoveEntity(IWorld world, IEntity entity);

        #endregion Public Methods
    }
}
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Worlds
{
    public interface IWorldMan
    {
        #region Public Events

        #endregion Public Events

        #region Public Methods

        IWorldBuilder Create();

        IWorld GetById(int id);

        IWorld GetByName(string name);

        void Remove(IWorld world);

        void Update(float dt);

        /// <summary>
        /// Request to add given entity to specified world.
        /// Entity will not be added immediately but at the end of each world update.
        /// An exception will be thrown if given entity already exists in world
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        /// <param name="world">ID of world to which entity should be added</param>
        void RequestAddEntity(IEntity entity, int worldId);

        /// <summary>
        /// Request to remove given entity from the world it is in.
        /// Entity will not be removed immediately but at the end of each world update.
        /// An exception will be thrown if given entity does not exist in this world.
        /// </summary>
        /// <param name="entity">Entity to be removed</param>
        void RequestRemoveEntity(IEntity entity);

        #endregion Public Methods
    }
}
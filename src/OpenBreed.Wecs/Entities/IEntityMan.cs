using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Entities
{
    public delegate void ComponentAdded(IEntity entity, Type componentType);

    public delegate void ComponentRemoved(IEntity entity, Type componentType);

    public interface IEntityMan
    {
        #region Public Events

        event ComponentAdded ComponentAdded;

        event ComponentRemoved ComponentRemoved;

        #endregion Public Events

        #region Public Methods

        IEnumerable<IEntity> GetByTag(string tag);

        IEnumerable<IEntity> Where(Func<IEntity, bool> predicate);

        IEntity GetById(int id);

        IEntity Create(string tag, List<IEntityComponent> initialComponents = null);

        /// <summary>
        /// Request to erase given entity from existance
        /// </summary>
        /// <param name="entity">Entity to erase</param>
        void RequestErase(IEntity entity);

        /// <summary>
        /// Perform cleanup
        /// </summary>
        void Cleanup();

        #endregion Public Methods
    }
}
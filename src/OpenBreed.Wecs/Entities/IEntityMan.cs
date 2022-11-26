using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Entities
{
    public delegate void ComponentAdded(IEntity entity, Type componentType);

    public delegate void ComponentRemoved(IEntity entity, Type componentType);

    public delegate void EnteringWorld(IEntity entity, int worldId);

    public delegate void LeavingWorld(IEntity entity);

    public interface IEntityMan
    {
        #region Public Events

        event ComponentAdded ComponentAdded;

        event ComponentRemoved ComponentRemoved;

        event EnteringWorld EnterWorldRequested;

        event LeavingWorld LeaveWorldRequested;

        #endregion Public Events

        #region Public Methods

        IEnumerable<IEntity> GetByTag(string tag);

        IEnumerable<IEntity> Where(Func<IEntity, bool> predicate);

        IEntity GetById(int id);

        IEntity Create(string tag, List<IEntityComponent> initialComponents = null);

        #endregion Public Methods
    }
}
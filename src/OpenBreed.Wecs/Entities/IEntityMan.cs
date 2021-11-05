using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Entities
{
    public delegate void ComponentAdded(Entity entity, Type componentType);

    public delegate void ComponentRemoved(Entity entity, Type componentType);

    public delegate void EnteringWorld(Entity entity, int worldId);

    public delegate void LeavingWorld(Entity entity);

    public interface IEntityMan
    {
        #region Public Events

        event ComponentAdded ComponentAdded;

        event ComponentRemoved ComponentRemoved;

        event EnteringWorld EnterWorldRequested;

        event LeavingWorld LeaveWorldRequested;

        #endregion Public Events

        #region Public Methods

        IEnumerable<Entity> GetByTag(object tag);

        IEnumerable<Entity> Where(Func<Entity, bool> predicate);

        Entity GetById(int id);

        Entity Create(List<IEntityComponent> initialComponents = null);

        void Subscribe<T>(Entity entity, Action<object, T> callback) where T : EventArgs;

        void Unsubscribe<T>(Entity entity, Action<object, T> callback) where T : EventArgs;

        #endregion Public Methods
    }
}
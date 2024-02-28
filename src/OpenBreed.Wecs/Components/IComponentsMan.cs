using System.Collections.Generic;
using System;

namespace OpenBreed.Wecs.Components
{
    public interface IComponentsMan
    {
        #region Public Methods

        void Add<TComponent>(int entityId, IEntityComponent component) where TComponent : class;

        bool Set<TComponent>(int id, TComponent component) where TComponent : class, IEntityComponent;

        bool Contains<TComponent>(int entityId);

        bool TryGet<TComponent>(int entityId, out TComponent component) where TComponent : class;

        T Get<T>(int entityId);

        bool Remove<T>(int entityId);

        ICollection<Type> Types(int id);

        #endregion Public Methods
    }
}
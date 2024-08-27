using OpenBreed.Core.Events;
using OpenBreed.Core.Interface.Events;
using OpenBreed.Wecs.Components;

namespace OpenBreed.Wecs.Events
{
    public interface IComponentChangedEvent : IEvent
    {
        #region Public Properties

        int EntityId { get; }

        #endregion Public Properties
    }

    public struct ComponentChangedEvent<TComponent> : IComponentChangedEvent where TComponent : IEntityComponent
    {
        #region Public Constructors

        public ComponentChangedEvent(int entityId, TComponent component, string propertyName)
        {
            EntityId = entityId;
            Component = component;
            ProperyName = propertyName;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }

        public TComponent Component { get; }

        public string ProperyName { get; }

        #endregion Public Properties
    }
}
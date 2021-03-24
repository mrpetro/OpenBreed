using System;

namespace OpenBreed.Wecs.Events
{
    public abstract class ComponentChangedEvent : EventArgs
    {
        public int EntityId { get; }

    }

    public class ComponentChangedEvent<TComponent> : ComponentChangedEvent
    {
        #region Public Constructors

        public ComponentChangedEvent(int entityId, TComponent component, string propertyName)
        {
            //EntityId = entityId;
            Component = component;
            ProperyName = propertyName;
        }

        #endregion Public Constructors

        #region Public Properties


        public TComponent Component { get; }

        public string ProperyName { get; }

        #endregion Public Properties
    }
}
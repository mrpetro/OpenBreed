using System;

namespace OpenBreed.Wecs.Events
{
    public class EntityComponentChangedEventArgs<TComponent> : EventArgs
    {
        #region Public Constructors

        public EntityComponentChangedEventArgs(TComponent component, string propertyName)
        {
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
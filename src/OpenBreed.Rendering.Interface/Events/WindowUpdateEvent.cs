using System;

namespace OpenBreed.Rendering.Interface.Events
{
    public class WindowUpdateEvent : EventArgs
    {
        #region Public Constructors

        public WindowUpdateEvent(IWindow window, float dt)
        {
            Window = window;
            Dt = dt;
        }

        #endregion Public Constructors

        #region Public Properties

        public IWindow Window { get; }

        public float Dt { get; }

        #endregion Public Properties
    }
}
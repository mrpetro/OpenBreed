using System;

namespace OpenBreed.Rendering.Interface.Events
{
    public class WindowLoadEvent : EventArgs
    {
        #region Public Constructors

        public WindowLoadEvent(IWindow window)
        {
            Window = window;
        }

        #endregion Public Constructors

        #region Public Properties

        public IWindow Window { get; }

        #endregion Public Properties
    }
}
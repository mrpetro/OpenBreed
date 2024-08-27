using OpenBreed.Rendering.Interface.Managers;
using System;

namespace OpenBreed.Rendering.Interface.Events
{
    public class WindowLoadEvent : EventArgs
    {
        #region Public Constructors

        public WindowLoadEvent(IWindow window, IRenderContext renderContext)
        {
            Window = window;
            RenderContext = renderContext;
        }

        #endregion Public Constructors

        #region Public Properties

        public IWindow Window { get; }
        public IRenderContext RenderContext { get; }

        #endregion Public Properties
    }
}
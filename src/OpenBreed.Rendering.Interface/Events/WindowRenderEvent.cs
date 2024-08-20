using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Events
{
    public class WindowRenderEvent : EventArgs
    {
        #region Public Constructors

        public WindowRenderEvent(IWindow window, float dt)
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
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Events
{
    public class WindowResizeEvent : EventArgs
    {
        #region Public Constructors

        public WindowResizeEvent(IWindow window, int width, int height)
        {
            Window = window;
            Width = width;
            Height = height;
        }

        #endregion Public Constructors

        #region Public Properties

        public IWindow Window { get; }
        public int Width { get; }
        public int Height { get; }

        #endregion Public Properties
    }
}
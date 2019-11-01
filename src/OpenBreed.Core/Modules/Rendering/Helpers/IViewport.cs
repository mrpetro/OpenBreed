using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Basic viewport interface
    /// </summary>
    public interface IViewport
    {
        void GetVisibleRectangle(out float left, out float bottom, out float right, out float top);

        void OnClientResize(Rectangle clientRectangle);

        /// <summary>
        /// Render this viewport content to the screen
        /// </summary>
        /// <param name="dt">Time step</param>
        void Render(float dt);
    }
}

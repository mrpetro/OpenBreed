using System;
using System.Collections.Generic;
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
        void OnClientResize(float x, float y, float width, float height);

        /// <summary>
        /// Draw viewport content to the screen
        /// </summary>
        void Draw();
    }
}

using OpenBreed.Core.Entities;
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
        /// <summary>
        /// Entity which view is being rendered to this viewport
        /// </summary>
        IEntity CameraEntity { get; }

        void GetVisibleRectangle(out float left, out float bottom, out float right, out float top);

        /// <summary>
        /// Render this viewport content to the screen
        /// </summary>
        /// <param name="dt">Time step</param>
        void Render(float dt);
    }
}

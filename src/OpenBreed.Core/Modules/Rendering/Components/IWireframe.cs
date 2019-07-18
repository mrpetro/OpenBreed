using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Wireframe render component interface
    /// </summary>
    public interface IWireframe : IEntityComponent
    {
        /// <summary>
        /// Thickness of wireframe lines
        /// </summary>
        float Thickness { get; set; }

        /// <summary>
        /// Color of wireframe lines
        /// </summary>
        Color4 Color { get; set; }
    }
}

using OpenBreed.Core.Common.Systems.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Camera component interface
    /// </summary>
    public interface ICameraComponent : IEntityComponent
    {
        /// <summary>
        /// Zoom factor of camera view
        /// </summary>
        float Zoom { get; set; }

        /// <summary>
        /// Brightness of camera view
        /// </summary>
        float Brightness { get; set; }
    }
}

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
        /// Zoom factor of camera
        /// </summary>
        float Zoom { get; set; }
    }
}

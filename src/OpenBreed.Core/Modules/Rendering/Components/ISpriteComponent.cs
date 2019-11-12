using OpenBreed.Core.Common.Systems.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Sprite render component interface
    /// </summary>
    public interface ISpriteComponent : IEntityComponent
    {
        /// <summary>
        /// Id of sprite atlas
        /// </summary>
        int AtlasId { get; set; }

        /// <summary>
        /// Id of sprite image from the atlas
        /// </summary>
        int ImageId { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        float Order { get; set; }
    }
}

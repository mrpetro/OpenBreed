using OpenBreed.Core.Modules.Rendering.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Tile render component interface
    /// </summary>
    public interface ITileComponent : IEntityComponent
    {
        /// <summary>
        /// Id of tile atlas
        /// </summary>
        int AtlasId { get; set; }

        /// <summary>
        /// Id of tile image from the atlas
        /// </summary>
        int ImageId { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        float Order { get; set; }
    }
}

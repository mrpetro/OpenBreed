using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems.Common.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Tile render component interface
    /// </summary>
    public interface ITile : IRenderComponent
    {
        /// <summary>
        /// Tile position
        /// </summary>
        Position Position { get; }

        /// <summary>
        /// Id of tile atlas
        /// </summary>
        int AtlasId { get; set; }

        /// <summary>
        /// Id of tile image from the atlas
        /// </summary>
        int ImageId { get; set; }
    }
}

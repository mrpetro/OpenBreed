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
        /// Width and height of this tile
        /// </summary>
        float Size { get; }

        /// <summary>
        /// Id of tile image from the atlas
        /// </summary>
        int ImageId { get; set; }
    }
}

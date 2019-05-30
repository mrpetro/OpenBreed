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
        /// Id of tile image from the atlas
        /// </summary>
        int ImageId { get; set; }
    }
}

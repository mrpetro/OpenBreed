using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems.Rendering.Components
{
    /// <summary>
    /// Sprite component interface
    /// </summary>
    public interface ISprite : IRenderComponent
    {
        /// <summary>
        /// Id of sprite image from the atlas
        /// </summary>
        int ImageId { get; set; }
    }
}

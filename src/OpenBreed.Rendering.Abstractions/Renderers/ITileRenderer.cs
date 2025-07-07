using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions.Renderers
{
    public interface ITileRenderer
    {
        #region Public Methods

        /// <summary>
        /// Render particular tile giving it's atlas and image ID
        /// </summary>
        /// <param name="atlasId">Atlas ID of rendered tile</param>
        /// <param name="imageId">Image ID of rendered tile</param>
        void Render(IRenderView view, int atlasId, int imageId);

        #endregion Public Methods
    }
}
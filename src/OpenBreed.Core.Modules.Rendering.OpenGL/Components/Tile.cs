using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Axis-aligned tile render component with same height and width
    /// </summary>
    internal class Tile : ITile
    {
        #region Private Fields

        #endregion Private Fields

        #region Internal Constructors

        internal Tile(int atlasId, int imageId)
        {
            AtlasId = atlasId;
            ImageId = imageId;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Id of tile atlas
        /// </summary>
        public int AtlasId { get; set; }

        /// <summary>
        /// Id of tile image from the atlas
        /// </summary>
        public int ImageId { get; set; }

        #endregion Public Properties
    }
}
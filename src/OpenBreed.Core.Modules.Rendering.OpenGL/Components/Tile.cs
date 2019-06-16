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

        private Position position;

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
        /// Tile position
        /// </summary>
        public Position Position { get { return position; } }

        /// <summary>
        /// Id of tile atlas
        /// </summary>
        public int AtlasId { get; set; }

        /// <summary>
        /// Id of tile image from the atlas
        /// </summary>
        public int ImageId { get; set; }

        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw this tile to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this tile will be rendered to</param>
        public void Draw(IViewport viewport)
        {
        }

        /// <summary>
        /// Initialize this component
        /// </summary>
        /// <param name="entity">Entity which this component belongs to</param>
        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().First();
        }

        /// <summary>
        /// Deinitialize this component
        /// </summary>
        /// <param name="entity">Entity which this component belongs to</param>
        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
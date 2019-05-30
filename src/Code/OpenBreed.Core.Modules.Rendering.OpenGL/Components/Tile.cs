using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Helpers;
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
        private ITileAtlas atlas;

        #endregion Private Fields

        #region Public Constructors

        internal Tile(ITileAtlas atlas, int imageId)
        {
            this.atlas = atlas;
            ImageId = imageId;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Id of tile image from the atlas
        /// </summary>
        public int ImageId { get; set; }

        public Type SystemType { get { return typeof(RenderSystem); } }

        /// <summary>
        /// Width and height of this tile
        /// </summary>
        public float Size { get { return atlas.TileSize; } }

        #endregion Public Properties

        #region Public Methods

        public void GetMapIndices(out int x, out int y)
        {
            x = (int)(position.Current.X / atlas.TileSize);
            y = (int)(position.Current.Y / atlas.TileSize);
        }

        /// <summary>
        /// Draw this tile to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this tile will be rendered to</param>
        public void Draw(IViewport viewport)
        {
            GL.PushMatrix();

            GL.Translate(position.Current.X, position.Current.Y, 0.0f);
            atlas.Draw(ImageId);

            GL.PopMatrix();
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
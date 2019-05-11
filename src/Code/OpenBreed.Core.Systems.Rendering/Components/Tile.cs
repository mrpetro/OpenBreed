using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Rendering.Helpers;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Core.Systems.Rendering.Components
{
    /// <summary>
    /// Class of axis-aligned tile graphics with same height and width
    /// </summary>
    public class Tile : IRenderComponent
    {
        #region Private Fields

        private Position position;
        private TileAtlas atlas;

        #endregion Private Fields

        #region Public Constructors

        public Tile(TileAtlas atlas, int imageId)
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
            x = (int)position.Current.X / atlas.TileSize;
            y = (int)position.Current.Y / atlas.TileSize;
        }

        /// <summary>
        /// Draw this tile to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this tile will be rendered to</param>
        public void Draw(Viewport viewport)
        {
            GL.PushMatrix();

            GL.Translate(position.Current.X, position.Current.Y, 0.0f);
            atlas.Draw(viewport, ImageId);

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
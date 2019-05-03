using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Rendering.Helpers;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Game.Rendering.Components
{
    public class Tile : IRenderComponent
    {
        #region Private Fields

        private Position position;
        private TileAtlas atlas;

        #endregion Private Fields

        #region Public Constructors

        public Tile(TileAtlas atlas, int tileId)
        {
            this.atlas = atlas;
            TileId = tileId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TileId { get; set; }

        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void GetMapIndices(out int x, out int y)
        {
            x = (int)position.X / atlas.TileSize;
            y = (int)position.Y / atlas.TileSize;
        }

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Draw(Viewport viewport)
        {
            GL.PushMatrix();

            GL.Translate(position.X, position.Y, 0.0f);
            atlas.Draw(viewport, TileId);

            GL.PopMatrix();
        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().First();
        }

        #endregion Public Methods
    }
}
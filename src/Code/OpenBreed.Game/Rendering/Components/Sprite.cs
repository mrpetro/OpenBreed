using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Rendering.Helpers;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Game.Rendering.Components
{
    internal class Sprite : IRenderComponent
    {
        #region Private Fields

        private SpriteAtlas atlas;
        private Position position;

        #endregion Private Fields

        #region Public Constructors

        public Sprite(SpriteAtlas atlas)
        {
            this.atlas = atlas;
        }

        #endregion Public Constructors

        #region Public Properties

        public int SpriteId { get; set; }
        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Draw(Viewport viewport)
        {
            GL.PushMatrix();

            GL.Translate(position.X, position.Y, 0.0f);
            atlas.Draw(viewport, SpriteId);

            GL.PopMatrix();
        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().First();
        }

        #endregion Public Methods
    }
}
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
        #region Public Fields

        public static uint[] indices = {
                                            0,1,2,
                                            0,2,3
                                       };

        #endregion Public Fields

        #region Private Fields

        private int ibo;
        private SpriteAtlas spriteAtlas;
        private int spriteId;
        private Transformation transformation;
        private int vbo;

        #endregion Private Fields

        #region Public Constructors

        public Sprite(SpriteAtlas spriteAtlas)
        {
            this.spriteAtlas = spriteAtlas;
            this.spriteId = 0;

            var vertices = spriteAtlas.GetVertices(spriteId);

            RenderTools.Create(vertices, indices, out vbo, out ibo);
        }

        #endregion Public Constructors

        #region Public Properties

        public int ImageId { get; set; }
        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
        }

        public void Draw(Viewport viewport)
        {
            GL.PushMatrix();

            GL.BindTexture(TextureTarget.Texture2D, spriteAtlas.Texture.Id);

            GL.MultMatrix(ref transformation.Value);
            RenderTools.Draw(viewport, vbo, ibo, 6);

            GL.PopMatrix();
        }

        public void Initialize(IEntity entity)
        {
            transformation = entity.Components.OfType<Transformation>().First();
        }

        #endregion Public Methods
    }
}
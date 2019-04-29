using OpenBreed.Game.Common;
using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Rendering.Helpers;
using System;
using OpenTK.Graphics.OpenGL;

namespace OpenBreed.Game.Rendering.Components
{
    internal class Sprite : IRenderComponent
    {
        public static uint[] indices = {
                                            0,1,2,
                                            0,2,3
                                       };

        private int vbo;
        private int ibo;

        #region Private Fields

        private Transformation transform;
        private SpriteAtlas spriteAtlas;
        private int spriteId;

        #endregion Private Fields

        #region Public Constructors

        public Sprite(SpriteAtlas spriteAtlas, int spriteId, Transformation transform)
        {
            this.spriteAtlas = spriteAtlas;
            this.spriteId = spriteId;
            this.transform = transform;

            var vertices = spriteAtlas.GetVertices(spriteId);

            RenderTools.Create(vertices, indices, out vbo, out ibo);
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IWorldSystem system)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(Viewport viewport)
        {
            GL.PushMatrix();

            GL.BindTexture(TextureTarget.Texture2D, spriteAtlas.Texture.Id);

            GL.MultMatrix(ref transform.Value);
            RenderTools.Draw(viewport, vbo, ibo, 6);

            GL.PopMatrix();
        }

        public void Initialize(IWorldSystem system)
        {
            //throw new System.NotImplementedException();
        }

        #endregion Public Methods
    }
}
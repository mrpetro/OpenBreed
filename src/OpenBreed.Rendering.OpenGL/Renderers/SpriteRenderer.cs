using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;

namespace OpenBreed.Rendering.OpenGL.Renderers
{
    public class SpriteRenderer : ISpriteRenderer
    {
        #region Private Fields

        private readonly SpriteMan spriteMan;

        #endregion Private Fields

        #region Public Constructors

        public SpriteRenderer(SpriteMan spriteMan)
        {
            this.spriteMan = spriteMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IRenderView view, Vector3 pos, Vector2 scale, Color4 color, int atlasId, int spriteId, bool ignoreScale = false)
        {
            var spriteAtlas = spriteMan.InternalGetById(atlasId);
            var vbo = spriteAtlas.GetSpriteVao(view.Context, spriteId);

            view.PushMatrix();

            try
            {
                var model = Matrix4.CreateScale(scale.X, scale.Y, 1.0f) * Matrix4.CreateTranslation(pos);

                if (ignoreScale)
                {
                    var viewScale = view.GetScale();
                    model = Matrix4.CreateScale(1 / viewScale, 1 / viewScale, 1.0f) * model;
                }

                view.Context.Primitives.SetTextureShader(
                    view,
                    spriteAtlas.Texture,
                    model,
                    color);

                GL.BindVertexArray(vbo);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                GL.BindVertexArray(0);
            }
            finally
            {
                view.PopMatrix();
            }
        }

        public void RenderBegin()
        {
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Texture2D);
        }

        public void RenderEnd()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods
    }
}
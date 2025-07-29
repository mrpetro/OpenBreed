using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Mathematics;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace OpenBreed.Rendering.OpenGL.Renderers
{
    public class PictureRenderer : IPictureRenderer
    {
        #region Private Fields

        private readonly PictureMan pictureMan;

        #endregion Private Fields

        #region Public Constructors

        public PictureRenderer(PictureMan pictureMan)
        {
            this.pictureMan = pictureMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IRenderView view, Vector3 pos, Vector2 scale, Color4 color, int imageId)
        {
            var picture = pictureMan.InternalGetById(imageId);

            view.PushMatrix();

            try
            {
                var model = Matrix4.CreateScale(scale.X, scale.Y, 1.0f) * Matrix4.CreateTranslation(pos);

                view.Context.Primitives.DrawSprite(
                    view,
                    picture.Texture,

                    model,
                    color);

                GL.BindVertexArray(picture.Vbo);
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
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Texture2D);
        }

        public void RenderEnd()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods
    }
}
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Mathematics;
using GL = OpenTK.Graphics.OpenGL;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class PictureRenderer : IPictureRenderer
    {
        #region Private Fields

        private readonly PictureMan pictureMan;
        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Public Constructors

        public PictureRenderer(PictureMan pictureMan,
                              IPrimitiveRenderer primitiveRenderer)
        {
            this.pictureMan = pictureMan;
            this.primitiveRenderer = primitiveRenderer;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IRenderView view, Vector3 pos, Vector2 scale, Color4 color, int imageId)
        {
            var picture = pictureMan.InternalGetById(imageId);

            view.PushMatrix();

            try
            {
                primitiveRenderer.DrawSprite(
                    view,
                    picture.Texture,
                    picture.Vbo,
                    pos,
                    scale,
                    color);
            }
            finally
            {
                view.PopMatrix();
            }
        }

        public void RenderBegin()
        {
            GL.GL.Enable(GL.EnableCap.Blend);
            GL.GL.BlendFunc(GL.BlendingFactor.One, GL.BlendingFactor.OneMinusSrcAlpha);
            GL.GL.AlphaFunc(GL.AlphaFunction.Greater, 0.0f);
            GL.GL.Enable(GL.EnableCap.Texture2D);
        }

        public void RenderEnd()
        {
            GL.GL.Disable(GL.EnableCap.Texture2D);
            GL.GL.Disable(GL.EnableCap.Blend);
        }

        #endregion Public Methods
    }
}
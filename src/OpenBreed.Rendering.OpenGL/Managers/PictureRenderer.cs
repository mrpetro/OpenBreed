using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Mathematics;
using GL = OpenTK.Graphics.OpenGL;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class PictureRenderer : IPictureRenderer
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

        public void Render(Vector3 pos, Vector2 size, int imageId)
        {
            var picture = pictureMan.InternalGetById(imageId);

            primitiveRenderer.PushMatrix();

            try
            {
                //primitiveRenderer.Translate(pos);

                //primitiveRenderer.DrawUnitBox(Matrix4.Identity, Color4.Red);


                primitiveRenderer.DrawSprite(picture.Texture, picture.Vbo, pos, size);

                //GL.GL.BindTexture(GL.TextureTarget.Texture2D, textureId);
                //RenderTools.Draw(vbo, ibo, 6);
                //GL.GL.BindTexture(GL.TextureTarget.Texture2D, 0);
            }
            finally
            {
                primitiveRenderer.PopMatrix();
            }
        }

        public void RenderBegin()
        {
            GL.GL.Enable(GL.EnableCap.Blend);
            GL.GL.Enable(GL.EnableCap.AlphaTest);
            GL.GL.BlendFunc(GL.BlendingFactor.One, GL.BlendingFactor.OneMinusSrcAlpha);
            GL.GL.AlphaFunc(GL.AlphaFunction.Greater, 0.0f);
            GL.GL.Enable(GL.EnableCap.Texture2D);
        }

        public void RenderEnd()
        {
            GL.GL.Disable(GL.EnableCap.Texture2D);
            GL.GL.Disable(GL.EnableCap.AlphaTest);
            GL.GL.Disable(GL.EnableCap.Blend);
        }

        #endregion Public Methods
    }
}
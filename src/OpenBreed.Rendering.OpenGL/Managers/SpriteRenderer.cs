using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Mathematics;
using GL = OpenTK.Graphics.OpenGL;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class SpriteRenderer : ISpriteRenderer
    {
        #region Private Fields

        private readonly uint[] indicesArray = {
                                            0,1,2,
                                            0,2,3
                                       };

        private readonly int ibo;
        private readonly SpriteMan spriteMan;

        #endregion Private Fields

        #region Public Constructors

        public SpriteRenderer(SpriteMan spriteMan)
        {
            RenderTools.CreateIndicesArray(indicesArray, out ibo);
            this.spriteMan = spriteMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(Vector3 pos, int atlasId, int imageId)
        {
            var spriteAtlas = spriteMan.InternalGetById(atlasId);
            var textureId = spriteAtlas.Texture.InternalId;
            var vbo = spriteAtlas.data[imageId].Vbo;

            GL.GL.PushMatrix();

            GL.GL.Translate(pos);

            GL.GL.BindTexture(GL.TextureTarget.Texture2D, textureId);
            RenderTools.Draw(vbo, ibo, 6);
            GL.GL.BindTexture(GL.TextureTarget.Texture2D, 0);

            GL.GL.PopMatrix();
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
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Mathematics;
using GL = OpenTK.Graphics.OpenGL;

namespace OpenBreed.Rendering.OpenGL.Managers
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

        public void Render(IRenderView view, Vector3 pos, Vector2 scale, Color4 color, int atlasId, int imageId, bool ignoreScale = false)
        {
            var spriteAtlas = spriteMan.InternalGetById(atlasId);
            var vbo = spriteAtlas.data[imageId].Vbo;

            view.PushMatrix();

            try
            {
                view.Context.Primitives.DrawSprite(
                    view,
                    spriteAtlas.Texture,
                    vbo,
                    pos,
                    scale,
                    color,
                    ignoreScale);
            }
            finally
            {
                view.PopMatrix();
            }
        }

        public void RenderBegin()
        {
            GL.GL.Enable(GL.EnableCap.Blend);
            GL.GL.Enable(GL.EnableCap.AlphaTest);
            GL.GL.BlendFunc(GL.BlendingFactor.One, GL.BlendingFactor.OneMinusSrcAlpha);
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
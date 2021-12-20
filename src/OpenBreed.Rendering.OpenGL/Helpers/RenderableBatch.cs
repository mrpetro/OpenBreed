using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class RenderableBatch : IRenderableBatch
    {
        #region Private Fields

        private const bool CLIPPING = true;

        private const int RENDER_MAX_DEPTH = 3;

        private readonly List<IRenderable> renderables = new List<IRenderable>();

        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Public Constructors

        public RenderableBatch(IPrimitiveRenderer primitiveRenderer)
        {
            this.primitiveRenderer = primitiveRenderer;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Add(IRenderable renderable)
        {
            renderables.Add(renderable);
        }

        public void Render(Matrix4 transform, Box2 clipBox, int depth, float dt)
        {
            GL.PushMatrix();

            try
            {
                GL.MultMatrix(ref transform);

                Render(clipBox, depth, dt);
            }
            finally
            {
                GL.PopMatrix();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Render(Box2 clipBox, int depth, float dt)
        {
            if (CLIPPING)
            {
                //Enable stencil buffer
                if (depth == 1)
                    GL.Enable(EnableCap.StencilTest);

                GL.ColorMask(false, false, false, false);
                GL.DepthMask(false);
                GL.StencilFunc(StencilFunction.Always, depth, depth);
                GL.StencilOp(StencilOp.Incr, StencilOp.Incr, StencilOp.Incr);

                // Draw black box
                GL.Color4(Color4.Black);
                primitiveRenderer.DrawBox(clipBox);

                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);
                GL.StencilFunc(StencilFunction.Equal, depth, depth);
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            }

            if (depth > RENDER_MAX_DEPTH)
                return;

            depth++;

            renderables.ForEach(renderable => renderable.Render(clipBox, depth, dt));

            if (CLIPPING)
            {
                GL.ColorMask(false, false, false, false);
                GL.DepthMask(false);
                GL.StencilFunc(StencilFunction.Always, depth, depth);
                GL.StencilOp(StencilOp.Decr, StencilOp.Decr, StencilOp.Decr);

                // Draw black box
                GL.Color4(Color4.Black);
                primitiveRenderer.DrawBox(clipBox);

                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);

                if (depth == 1)
                    GL.Disable(EnableCap.StencilTest);
            }
        }

        #endregion Private Methods
    }
}
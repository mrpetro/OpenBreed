using OpenBreed.Game.Entities.Components;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace OpenBreed.Game.Rendering
{
    public class Viewport
    {
        #region Public Constructors

        public Viewport(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Bottom { get { return Y; } }
        public float Height { get; set; }
        public float Left { get { return X; } }
        public float Right { get { return X + Width; } }
        public float Top { get { return Y + Height; } }
        public Camera View { get; set; }
        public float Width { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Draw(RenderSystem system)
        {
            const bool clip = true;

            if (clip)
            {
                //Clear stencil buffer before drawing in it
                GL.Clear(ClearBufferMask.StencilBufferBit);
                //Enable stencil buffer
                GL.Enable(EnableCap.StencilTest);

                GL.StencilFunc(StencilFunction.Equal, 0x1, 0x1);
                GL.StencilOp(StencilOp.Replace, StencilOp.Replace, StencilOp.Replace);
                //Draw rectangle shape which will clip anything inside viewport
                GL.Color3(0.0f, 0.0f, 0.0f);

                GL.Begin(PrimitiveType.Polygon);                            // Use A Quad For Each Character
                GL.Vertex3(Left, Top, 0.0);
                GL.Vertex3(Left, Bottom, 0.0);
                GL.Vertex3(Right, Bottom, 0.0);
                GL.Vertex3(Right, Top, 0.0);
                GL.End();

                GL.StencilFunc(StencilFunction.Equal, 0x1, 0x1);
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            }

            var transform = View.GetTransform();
            transform.Invert();
            GL.MultMatrix(ref transform);

            system.Draw(this);

            if (clip)
            {
                //Disable stencil buffer here
                GL.Disable(EnableCap.StencilTest);
            }

            transform.Invert();
            GL.MultMatrix(ref transform);

            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(Left, Top, 0.0);
            GL.Vertex3(Left, Bottom, 0.0);
            GL.Vertex3(Right, Bottom, 0.0);
            GL.Vertex3(Right, Top, 0.0);
            GL.End();
        }

        public void OnResize(Rectangle clientRectangle)
        {
        }

        public void Update()
        {
        }

        #endregion Public Methods
    }
}
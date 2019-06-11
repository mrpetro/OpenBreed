using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// This class defines viewport using screen coordinates
    /// </summary>
    public class Viewport : IViewport
    {
        #region Public Constructors

        /// <summary>
        /// Viewport constructor with screen coordinates and size of view box
        /// </summary>
        /// <param name="x">X screen coordinate of left bottom viewport corner</param>
        /// <param name="y">Y screen coordinate of left bottom viewport corner</param>
        /// <param name="width">Width of defined viewport</param>
        /// <param name="height">Height of defined viewport</param>
        public Viewport(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            Clipping = true;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///  Flag to draw border box of viewport
        /// </summary>
        public bool DrawBorder { get; set; }

        /// <summary>
        /// Flag to clip any graphics that is outside of viewport box
        /// </summary>
        public bool Clipping { get; set; }

        /// <summary>
        /// Viewport background color
        /// </summary>
        public Color4 BackgroundColor { get; set; }

        /// <summary>
        /// Draw viewport background if this flag is true
        /// </summary>
        public bool DrawBackgroud { get; set; }

        /// <summary>
        /// Bottom edge screen coordinate of this viewport
        /// </summary>
        public float Bottom { get { return Y; } }

        /// <summary>
        /// Camera that is attached to this viewport
        /// </summary>
        public Camera Camera { get; set; }

        /// <summary>
        /// Height of this viewport
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Left edge screen coordinate of this viewport
        /// </summary>
        public float Left { get { return X; } }

        /// <summary>
        /// Right edge screen coordinate of this viewport
        /// </summary>
        public float Right { get { return X + Width; } }

        /// <summary>
        /// Top edge screen coordinate of this viewport
        /// </summary>
        public float Top { get { return Y + Height; } }

        /// <summary>
        /// Width of this viewport
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// X screen coordinate of left bottom viewport corner
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y screen coordinate of left bottom viewport corner
        /// </summary>
        public float Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        public bool TestScreenCoords(Vector2 point)
        {
            if (point.X < Left)
                return false;

            if (point.X > Right)
                return false;

            if (point.Y < Bottom)
                return false;

            if (point.Y > Top)
                return false;

            return true;
        }

        /// <summary>
        /// Perform draw of render system from world that current camera is looking at
        /// </summary>
        public void Draw()
        {
            GL.Translate(X, Y, 0.0f);

            if (Clipping)
            {
                //Clear stencil buffer before drawing in it
                GL.Clear(ClearBufferMask.StencilBufferBit);
                //Enable stencil buffer
                GL.Enable(EnableCap.StencilTest);

                GL.StencilFunc(StencilFunction.Equal, 0x1, 0x1);
                GL.StencilOp(StencilOp.Replace, StencilOp.Replace, StencilOp.Replace);
                //Draw rectangle shape which will clip anything inside viewport
                GL.Color3(0.0f, 0.0f, 0.0f);

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(0, Height, 0.0);
                GL.Vertex3(0, 0, 0.0);
                GL.Vertex3(Width, 0, 0.0);
                GL.Vertex3(Width, Height, 0.0);
                GL.End();

                GL.StencilFunc(StencilFunction.Equal, 0x1, 0x1);
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            }

            if (DrawBackgroud)
                DrawBackground();

            Camera.RenderTo(this);

            if (Clipping)
            {
                GL.Disable(EnableCap.StencilTest);
            }

            if (DrawBorder)
            {
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color4(1.0f, 0.0f, 0.0f, 1.0f);
                GL.Vertex3(0, Height, 0.0);
                GL.Vertex3(0, 0, 0.0);
                GL.Vertex3(Width, 0, 0.0);
                GL.Vertex3(Width, Height, 0.0);
                GL.End();
            }

            GL.Translate(-X, -Y, 0.0f);
        }

        public Matrix4 GetTransform()
        {
            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(X, Y, 0));
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(Width / 2.0f, Height / 2.0f, 1.0f));
            return transform;
        }

        public Vector2 GetWorldCoords(Vector2 screenCoords)
        {
            var cameraT = Camera.GetTransform();
            cameraT = Matrix4.Mult(cameraT,GetTransform());
            cameraT.Invert();

            var pointLB = new Vector4(screenCoords.X, screenCoords.Y, 0.0f, 1.0f);
            pointLB = Vector4.Transform(pointLB, cameraT);

            return new Vector2(pointLB.X, pointLB.Y);
        }

        public void GetVisibleRectangle(out float left, out float bottom, out float right, out float top)
        {
            var transf = Camera.GetTransform();
            transf.Invert();
            var pointLB = new Vector4(-Width / 2.0f, -Height / 2.0f, 0.0f, 1.0f);
            var pointRT = new Vector4(Width / 2.0f, Height / 2.0f, 0.0f, 1.0f);
            pointLB = Vector4.Transform(pointLB, transf);
            pointRT = Vector4.Transform(pointRT, transf);

            left = pointLB.X;
            bottom = pointLB.Y;
            right = pointRT.X;
            top = pointRT.Y;
        }

        public void OnResize(Rectangle clientRectangle)
        {
        }

        public void Update()
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void DrawBackground()
        {
            //Draw background for this viewport
            GL.Color4(BackgroundColor);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, Height, 0.0);
            GL.Vertex3(0, 0, 0.0);
            GL.Vertex3(Width, 0, 0.0);
            GL.Vertex3(Width, Height, 0.0);
            GL.End();
        }

        #endregion Private Methods
    }
}
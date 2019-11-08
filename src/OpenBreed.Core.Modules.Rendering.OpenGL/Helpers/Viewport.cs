using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// This class defines viewport using client(window or screen) coordinates
    /// </summary>
    public class Viewport : IViewport
    {
        #region Private Fields

        private const float ZOOM_BASE = 1.0f / 512.0f;

        private ICore core;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Viewport constructor using screen percent coordinates and size
        /// </summary>
        /// <param name="x">X client percent coordinate of left bottom viewport corner</param>
        /// <param name="y">Y client percent coordinate of left bottom viewport corner</param>
        /// <param name="width">Width of defined viewport</param>
        /// <param name="height">Height of defined viewport</param>
        internal Viewport(ICore core, float x, float y, float width, float height)
        {
            this.core = core;
            X = x;
            Y = y;
            Width = width;
            Height = height;

            Clipping = true;
        }

        #endregion Internal Constructors

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
        /// Bottom edge client coordinate of this viewport
        /// </summary>
        public float Bottom { get { return Y; } }

        /// <summary>
        /// Camera that is attached to this viewport
        /// </summary>
        public CameraEntity Camera { get; set; }

        public ICamera CameraEx { get; set; }

        /// <summary>
        /// Height of this viewport
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Left edge client coordinate of this viewport
        /// </summary>
        public float Left { get { return X; } }

        /// <summary>
        /// Right edge client coordinate of this viewport
        /// </summary>
        public float Right { get { return X + Width; } }

        /// <summary>
        /// Top edge client coordinate of this viewport
        /// </summary>
        public float Top { get { return Y + Height; } }

        /// <summary>
        /// Width of this viewport
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// X client coordinate of left bottom viewport corner
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y client coordinate of left bottom viewport corner
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Aspect ratio of this viewport
        /// </summary>
        public float Ratio { get { return Width / Height; } }

        /// <summary>
        /// Local transformation matrix of this viewport
        /// </summary>
        public Matrix4 Transform
        {
            get
            {
                var transform = Matrix4.Identity;
                transform = Matrix4.Mult(transform, Matrix4.CreateScale(Width, Height, 1.0f));
                transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(X, Y, 0.0f));
                return transform;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public bool TestClientCoords(Vector2 point)
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
        /// Render this viewport content to the client
        /// </summary>
        /// <param name="dt">Time step</param>
        public void Render(float dt)
        {
            GL.PushMatrix();

            //Apply viewport transformation matrix
            var transform = Transform;
            GL.MultMatrix(ref transform);

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
                GL.Vertex3(0, 1.0f, 0.0);
                GL.Vertex3(0, 0, 0.0);
                GL.Vertex3(1.0f, 0, 0.0);
                GL.Vertex3(1.0f, 1.0f, 0.0);
                GL.End();

                GL.StencilFunc(StencilFunction.Equal, 0x1, 0x1);
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            }

            if (DrawBackgroud)
                DrawBackground();

            DrawCameraView(dt);

            if (Clipping)
            {
                GL.Disable(EnableCap.StencilTest);
            }

            if (DrawBorder)
            {
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color4(1.0f, 0.0f, 0.0f, 1.0f);
                GL.Vertex3(0, 1.0f, 0.0);
                GL.Vertex3(0, 0, 0.0);
                GL.Vertex3(1.0f, 0, 0.0);
                GL.Vertex3(1.0f, 1.0f, 0.0);
                GL.End();
            }

            GL.PopMatrix();
        }

        /// <summary>
        /// This will return camera tranformation matrix which includes aspect ratio correction
        /// </summary>
        /// <returns>Camera transformation matrix</returns>
        public Matrix4 GetCameraTransform()
        {
            var transform = Camera.Transform;

            var ratio = core.ClientRatio * Ratio;
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f, ratio, 1.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(ZOOM_BASE, ZOOM_BASE, 1.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f));

            return transform;
        }

        public Vector2 FromClientPoint(Vector2 point)
        {
            var newPos4 = new Vector4(point) { W = 1 };
            var invT = Matrix4.Invert(Transform);
            newPos4 = Vector4.Transform(newPos4, invT);
            return new Vector2(newPos4.X, newPos4.Y);
        }

        public Vector4 ViewportToWorldPoint(Vector4 point)
        {
            var cameraT = GetCameraTransform();
            cameraT.Invert();
            return Vector4.Transform(point, cameraT);
        }

        public Vector2 ClientToWorldPoint(Vector2 point)
        {
            var cameraT = GetCameraTransform();
            cameraT = Matrix4.Mult(cameraT, Transform);
            cameraT.Invert();

            var pointLB = new Vector4(point.X, point.Y, 0.0f, 1.0f);
            pointLB = Vector4.Transform(pointLB, cameraT);

            return new Vector2(pointLB.X, pointLB.Y);
        }

        public Vector2 ClientToWorldVector(Vector2 vector)
        {
            var cameraT = GetCameraTransform();
            cameraT = Matrix4.Mult(cameraT, Transform);
            cameraT.Invert();

            var pointLB = new Vector4(vector.X, vector.Y, 0.0f, 0.0f);
            pointLB = Vector4.Transform(pointLB, cameraT);

            return new Vector2(pointLB.X, pointLB.Y);
        }

        public void GetVisibleRectangle(out float left, out float bottom, out float right, out float top)
        {
            var pointLB = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            var pointRT = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
            pointLB = ViewportToWorldPoint(pointLB);
            pointRT = ViewportToWorldPoint(pointRT);
            //pointLB = Vector4.Transform(pointLB, transf);
            //pointRT = Vector4.Transform(pointRT, transf);

            left = pointLB.X;
            bottom = pointLB.Y;
            right = pointRT.X;
            top = pointRT.Y;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// This will render world part currently visible by the camera into given viewport
        /// </summary>
        /// <param name="dt">Time step</param>
        private void DrawCameraView(float dt)
        {
            GL.PushMatrix();

            //Apply camera transformation matrix
            var transform = GetCameraTransform();
            GL.MultMatrix(ref transform);

            Camera.World.Systems.OfType<IRenderableSystem>().ForEach(item => item.Render(this, dt));

            GL.PopMatrix();
        }

        private void DrawBackground()
        {
            //Draw background for this viewport
            GL.Color4(BackgroundColor);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 1.0f, 0.0);
            GL.Vertex3(0, 0, 0.0);
            GL.Vertex3(1.0f, 0, 0.0);
            GL.Vertex3(1.0f, 1.0f, 0.0);
            GL.End();
        }

        #endregion Private Methods
    }
}
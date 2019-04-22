﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace OpenBreed.Game.Rendering.Helpers
{
    /// <summary>
    /// This class defines viewport using screen coordinates
    /// </summary>
    public class Viewport
    {
        #region Public Fields

        /// <summary>
        /// This will draw border box of viewport
        /// </summary>
        public const bool BORDER = true;

        /// <summary>
        /// This will clip any graphics that is outside of viewport box
        /// </summary>
        public const bool CLIPPING = true;

        #endregion Public Fields

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
        }

        public void GetVisibleRectangle(out float left, out float bottom, out float right, out float top)
        {
            var transf = Camera.GetTransform();
            var pointLB = new Vector3(Left, Bottom, 0.0f);
            var pointRT = new Vector3(Right, Top, 0.0f);

            var tLB = Matrix4.CreateTranslation(pointLB);
            var tRT = Matrix4.CreateTranslation(pointRT);

            tLB = Matrix4.Mult(tLB, transf);
            tRT = Matrix4.Mult(tRT, transf);

            pointLB = tLB.ExtractTranslation();
            pointRT = tRT.ExtractTranslation();

            left = pointLB.X;
            bottom = pointLB.Y;
            right = pointRT.X;
            top = pointRT.Y;
        }

        #endregion Public Constructors

        #region Public Properties

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

        /// <summary>
        /// This will perform drawing of render system that is given in the argumnet
        /// </summary>
        /// <param name="system">Render system to draw</param>
        public void Draw(RenderSystem system)
        {
            if (CLIPPING)
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

            var transform = Camera.GetTransform();
            transform.Invert();
            GL.MultMatrix(ref transform);

            system.Draw(this);

            if (CLIPPING)
            {
                GL.Disable(EnableCap.StencilTest);
            }

            transform.Invert();
            GL.MultMatrix(ref transform);

            if (BORDER)
            {
                GL.Color4(1.0f, 0.0f, 0.0f, 1.0f);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Left, Top, 0.0);
                GL.Vertex3(Left, Bottom, 0.0);
                GL.Vertex3(Right, Bottom, 0.0);
                GL.Vertex3(Right, Top, 0.0);
                GL.End();
            }
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
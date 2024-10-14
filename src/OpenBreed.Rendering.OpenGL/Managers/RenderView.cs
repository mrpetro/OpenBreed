using Microsoft.Extensions.Logging;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class RenderView : IRenderView
    {
        #region Private Fields

        private readonly Stack<Matrix4> modelMatrixStack = new Stack<Matrix4>();
        private readonly Stack<IPalette> paletteStack = new Stack<IPalette>();
        private readonly HostCoordinateSystemConverter hostCoordinateSystemConverter;
        private readonly RenderDelegate renderer;
        private readonly Box2 boxNormalized;
        private IPalette currentPalette;
        private Matrix4 projection = Matrix4.Identity;

        #endregion Private Fields

        #region Public Constructors

        public RenderView(
            IRenderContext context,
            HostCoordinateSystemConverter hostCoordinateSystemConverter,
            RenderDelegate renderer,
            Box2 boxNormalized)
        {
            Context = context;
            this.hostCoordinateSystemConverter = hostCoordinateSystemConverter;
            this.renderer = renderer;
            this.boxNormalized = boxNormalized;
        }

        #endregion Public Constructors

        #region Public Events

        public event ResizeDelegate Resized;

        #endregion Public Events

        #region Public Properties

        public Box2i Box { get; private set; }

        public IRenderContext Context { get; }
        public IFontMan Fonts { get; }
        public ResizeDelegate Resizer { get; set; }
        public Matrix4 View { get; set; } = Matrix4.Identity;
        public Matrix4 Projection => projection;
        public IPalette CurrentPalette => currentPalette;

        #endregion Public Properties

        #region Public Methods

        public void RenderViewport(bool drawBorder, bool drawBackground, Color4 backgroundColor, Matrix4 viewportTransform, Action func)
        {
            PushMatrix();

            try
            {
                MultMatrix(viewportTransform);

                if (drawBackground)
                    Context.Primitives.DrawUnitRectangle(
                        this,
                        Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f),
                        backgroundColor,
                        filled: true);

                if (drawBorder)
                    Context.Primitives.DrawUnitRectangle(
                        this,
                        Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f),
                        Color4.Red,
                        filled: false);

                func.Invoke();
            }
            finally
            {
                PopMatrix();
            }
        }

        public void EnableAlpha()
        {
            OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.AlphaTest);
            OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
        }

        public void DisableAlpha()
        {
            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.AlphaTest);
        }

        public Vector2i GetHostToViewCoords(Vector2i point)
        {
            point = hostCoordinateSystemConverter.Invoke(point);

            return point - Box.Min;
        }

        public Vector4 GetViewToWorldCoords(Vector2i point)
        {
            var mat = View;
            mat.Invert();
            var coordsT = new Vector4(point.X, point.Y, 0.0f, 1.0f) * mat;
            coordsT.W = 1.0f / coordsT.W;
            coordsT.X *= coordsT.W;
            coordsT.Y *= coordsT.W;
            coordsT.Z *= coordsT.W;
            return coordsT;
        }

        public Vector2i GetWorldToViewCoords(Vector2 point)
        {
            var mat = View;
            var coordsT = new Vector4(point.X, point.Y, 0.0f, 1.0f) * mat;
            coordsT.W = 1.0f / coordsT.W;
            coordsT.X *= coordsT.W;
            coordsT.Y *= coordsT.W;
            coordsT.Z *= coordsT.W;
            return new Vector2i((int)coordsT.X, (int)coordsT.Y);
        }

        public Box2 GetViewToWorldCoords(Box2i box)
        {
            var wMin = GetViewToWorldCoords(box.Min);
            var wMax = GetViewToWorldCoords(box.Max);
            return new Box2(new Vector2(wMin.X, wMin.Y), new Vector2(wMax.X, wMax.Y));
        }

        public Box2i GetWorldToViewCoords(Box2 box)
        {
            var wMin = GetWorldToViewCoords(box.Min);
            var wMax = GetWorldToViewCoords(box.Max);
            return new Box2i(new Vector2i(wMin.X, wMin.Y), new Vector2i(wMax.X, wMax.Y));
        }

        public Vector4 GetHostToWorldCoords(Vector2i point)
        {
            point = GetHostToViewCoords(point);
            return GetViewToWorldCoords(point);
        }

        public void MultMatrix(Matrix4 transform)
        {
            View = transform * View;
        }

        public void PopMatrix()
        {
            View = modelMatrixStack.Pop();
        }

        public void PopPalette()
        {
            currentPalette = paletteStack.Pop();
        }

        public void PushMatrix()
        {
            modelMatrixStack.Push(View);
        }

        public void PushPalette()
        {
            paletteStack.Push(currentPalette);
        }

        public void SetPalette(IPalette palette)
        {
            currentPalette = palette;
        }

        public void SetProjection(Matrix4 matrix)
        {
            projection = matrix;
        }

        public virtual void Reset()
        {
            View = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
        }

        public void Translate(Vector3 vec)
        {
            View = Matrix4.CreateTranslation(vec) * View;
        }

        public void Translate(float x, float y, float z) => Translate(new Vector3(x, y, z));

        public void Scale(float value) => Scale(value, value);

        public void Scale(float x, float y)
        {
            View = Matrix4.CreateScale(x, y, 1.0f) * View;
        }

        #endregion Public Methods

        #region Internal Methods

        internal virtual void OnRender(float dt)
        {
            GL.Viewport(Box);

            renderer?.Invoke(this, Matrix4.Identity, dt);
        }

        internal virtual void OnResize(int width, int height)
        {
            var min = new Vector2i(width, height) * boxNormalized.Min;
            var max = new Vector2i(width, height) * boxNormalized.Max;

            Box = new Box2i(((Vector2i)min), (Vector2i)max);

            SetProjection(Matrix4.CreateOrthographicOffCenter(0, Box.Size.X, 0, Box.Size.Y, -100.0f, 100.0f));

            View = Matrix4.Identity;

            Resizer?.Invoke(this, width, height);

            Resized?.Invoke(this, width, height);
        }

        #endregion Internal Methods
    }
}
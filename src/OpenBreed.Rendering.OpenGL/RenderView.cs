using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace OpenBreed.Rendering.OpenGL
{
    public class RenderView : IRenderView
    {
        #region Private Fields

        private readonly Stack<Matrix4> modelMatrixStack = new Stack<Matrix4>();
        private readonly Stack<IPalette> paletteStack = new Stack<IPalette>();
        private readonly Stack<Box2i> clipBoxStack = new Stack<Box2i>();
        private readonly HostCoordinateSystemConverter hostCoordinateSystemConverter;
        private readonly Box2 boxNormalized;
        private readonly OpenTKRenderContext context;
        private IPalette currentPalette;
        private Matrix4 projection = Matrix4.Identity;
        private readonly List<IRenderLayer> layers = new List<IRenderLayer>();

        #endregion Private Fields

        #region Public Constructors

        public RenderView(
            OpenTKRenderContext context,
            HostCoordinateSystemConverter hostCoordinateSystemConverter,
            Box2 boxNormalized,
            int viewId)
        {
            this.context = context;
            this.hostCoordinateSystemConverter = hostCoordinateSystemConverter;
            this.boxNormalized = boxNormalized;
            Id = viewId;
        }

        #endregion Public Constructors

        #region Public Events

        public event ViewResizeHandler Resized;

        public event ViewRenderHandler Rendering;

        public event ViewCursorEnterHandler CursorEnter;

        public event ViewCursorLeaveHandler CursorLeave;

        public event ViewCursorDownHandler CursorDown;

        public event ViewCursorUpHandler CursorUp;

        public event ViewCursorMoveHandler CursorMove;

        public event ViewCursorWheelHandler CursorWheel;

        public event ViewTextInputHandler TextInput;

        public event ViewKeyDownHandler KeyDown;

        public event ViewKeyUpHandler KeyUp;

        #endregion Public Events

        #region Public Properties

        public Box2i Box { get; private set; }

        public IRenderContext Context => context;
        public int Id { get; }
        public IFontMan Fonts { get; }
        public Matrix4 View { get; set; } = Matrix4.Identity;
        public Matrix4 Projection => projection;
        public IPalette CurrentPalette => currentPalette;

        #endregion Public Properties

        #region Public Methods

        public bool Activate()
        {
            return context.ActivateView(this);
        }

        public bool Deactivate()
        {
            return context.DeactivateView(this);
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

        public Vector2i FromHostPoint(Vector2i hostPoint)
        {
            hostPoint = hostCoordinateSystemConverter.Invoke(hostPoint);

            return hostPoint - Box.Min;
        }

        public Vector4 ToWorldPoint(Vector2i viewPoint)
        {
            var mat = View;
            mat.Invert();
            var worldPoint = new Vector4(viewPoint.X, viewPoint.Y, 0.0f, 1.0f) * mat;
            worldPoint.W = 1.0f / worldPoint.W;
            worldPoint.X *= worldPoint.W;
            worldPoint.Y *= worldPoint.W;
            worldPoint.Z *= worldPoint.W;
            return worldPoint;
        }

        public Vector2i FromWorldPoint(Vector2 worldPoint)
        {
            var mat = View;
            var viewPoint = new Vector4(worldPoint.X, worldPoint.Y, 0.0f, 1.0f) * mat;
            viewPoint.W = 1.0f / viewPoint.W;
            viewPoint.X *= viewPoint.W;
            viewPoint.Y *= viewPoint.W;
            viewPoint.Z *= viewPoint.W;
            return new Vector2i((int)viewPoint.X, (int)viewPoint.Y);
        }

        public Box2 ToWorldBox(Box2i viewBox)
        {
            var wMin = ToWorldPoint(viewBox.Min);
            var wMax = ToWorldPoint(viewBox.Max);
            return new Box2(new Vector2(wMin.X, wMin.Y), new Vector2(wMax.X, wMax.Y));
        }

        public Box2i FromWorldBox(Box2 worldBox)
        {
            var wMin = FromWorldPoint(worldBox.Min);
            var wMax = FromWorldPoint(worldBox.Max);
            return new Box2i(new Vector2i(wMin.X, wMin.Y), new Vector2i(wMax.X, wMax.Y));
        }

        public Vector4 FromHostToWorldPoint(Vector2i hostPoint)
        {
            hostPoint = FromHostPoint(hostPoint);
            return ToWorldPoint(hostPoint);
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

        public IRenderLayer CreateLayer(ViewRenderHandler renderHandler)
        {
            var newLayer = new RenderLayer(renderHandler);
            layers.Add(newLayer);
            return newLayer;
        }

        #endregion Public Methods

        #region Internal Methods

        internal virtual void OnRender(float dt)
        {
            GL.ViewportIndexed(Id, Box.Min.X, Box.Min.Y, Box.Size.X, Box.Size.Y);

            Rendering?.Invoke(this, dt);

            RenderLayers(dt);
        }

        private void RenderLayers(float dt)
        {

            for (int i = 0; i < layers.Count; i++)
            {
                RenderLayer(layers[i], dt);
            }
        }

        private void RenderLayer(IRenderLayer layer, float dt)
        {
            PushMatrix();

            try
            {
                layer.Render(this, dt);
            }
            finally
            {
                PopMatrix();
            }
        }


        internal virtual void OnCursorWheel(int cursorId, Vector2i cursorPosition, int wheelDelta)
        {
            CursorWheel?.Invoke(this, cursorId, cursorPosition, wheelDelta);
        }

        internal void OnCursorUp(int cursorId, Vector2i cursorPosition, CursorKey cursorKey)
        {
            CursorUp?.Invoke(this, cursorId, cursorPosition, cursorKey);
        }

        internal virtual void OnCursorDown(int cursorId, Vector2i cursorPosition, CursorKey cursorKey)
        {
            CursorDown?.Invoke(this, cursorId, cursorPosition, cursorKey);
        }

        internal void OnCursorMove(int cursorId, Vector2i cursorPosition, BitArray cursorKeyStates, Abstractions.Events.KeyModifiers modifiers)
        {
            CursorMove?.Invoke(this, cursorId, cursorPosition, cursorKeyStates, modifiers);
        }

        internal void OnCursorEnter(int cursorId, Vector2i cursorPosition)
        {
            CursorEnter?.Invoke(this, cursorId, cursorPosition);
        }

        internal void OnCursorLeave(int cursorId, Vector2i cursorPosition)
        {
            CursorLeave?.Invoke(this, cursorId, cursorPosition);
        }

        internal void OnTextInput(string text)
        {
            TextInput?.Invoke(this, text);
        }

        internal void OnKeyDown(Abstractions.Events.Keys key, Abstractions.Events.KeyModifiers modifiers)
        {
            KeyDown?.Invoke(this, key, modifiers);
        }

        internal void OnKeyUp(Abstractions.Events.Keys key, Abstractions.Events.KeyModifiers modifiers)
        {
            KeyUp?.Invoke(this, key, modifiers);
        }

        internal virtual void OnResize()
        {
            var size = context.Size;

            var min = size * boxNormalized.Min;
            var max = size * boxNormalized.Max;

            Box = new Box2i((Vector2i)min, (Vector2i)max);

            SetProjection(Matrix4.CreateOrthographicOffCenter(0, Box.Size.X, 0, Box.Size.Y, -100.0f, 100.0f));

            Resized?.Invoke(this, size.X, size.Y);
        }

        #endregion Internal Methods
    }
}
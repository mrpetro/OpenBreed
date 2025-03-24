using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Extensions;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class RenderView : IRenderView
    {
        #region Private Fields

        private readonly Stack<Matrix4> modelMatrixStack = new Stack<Matrix4>();
        private readonly Stack<IPalette> paletteStack = new Stack<IPalette>();
        private readonly Stack<Box2i> clipBoxStack = new Stack<Box2i>();
        private readonly HostCoordinateSystemConverter hostCoordinateSystemConverter;
        private readonly Box2 boxNormalized;
        private IPalette currentPalette;
        private Matrix4 projection = Matrix4.Identity;

        #endregion Private Fields

        #region Public Constructors

        public RenderView(
            IRenderContext context,
            HostCoordinateSystemConverter hostCoordinateSystemConverter,
            Box2 boxNormalized,
            int viewId)
        {
            Context = context;
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

        public event ViewKeyboardKeyHandler KeyDown;

        public event ViewKeyboardKeyHandler KeyUp;

        #endregion Public Events

        #region Public Properties

        public Box2i Box { get; private set; }

        public IRenderContext Context { get; }
        public int Id { get; }
        public IFontMan Fonts { get; }
        public ViewResizeHandler Resizer { get; set; }
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

        public void Translate(Vector2 vec)
        {
            View = Matrix4.CreateTranslation(new Vector3(vec)) * View;
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
            GL.ViewportIndexed(Id,Box.Min.X, Box.Min.Y, Box.Size.X, Box.Size.Y);

            Rendering?.Invoke(this, Matrix4.Identity, dt);
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

        internal void OnCursorMove(int cursorId, Vector2i cursorPosition)
        {
            CursorMove?.Invoke(this, cursorId, cursorPosition);
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

        internal void OnKeyDown(Interface.Events.Keys key, Interface.Events.KeyModifiers modifiers)
        {
            KeyDown?.Invoke(this, key, modifiers);
        }

        internal void OnKeyUp(Interface.Events.Keys key, Interface.Events.KeyModifiers modifiers)
        {
            KeyUp?.Invoke(this, key, modifiers);
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

        public void RenderWinScissor(IWin win)
        {
            GL.Enable(EnableCap.ScissorTest);
            GL.Disable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Greater);

            RenderWinScissor(win, Vector2.Zero, layer: 0);

            Dumper.DumpWhenRequested(@"D:\Projects\Output\OpenBreed\Dumps", "dmp");
        }

        private void RenderWinScissor(IWin win, Vector2 origin, int layer)
        {
            PushMatrix();

            Translate(win.Pos);

            var newPos = origin + win.Pos;

            var stencilFunction = layer > 0 ? StencilFunction.Lequal : StencilFunction.Always;




            GL.StencilFunc(stencilFunction, layer, 0xff);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
            DrawBoxMask(Vector2.Zero, win.Body.Inflated(win.Margin), 0);
            GL.StencilMask(0x00);
            GL.ColorMask(true, true, true, true);
            DrawBox(Vector2.Zero, win.Body, win.BorderColor, win.ForegroundColor);
            GL.ColorMask(false, false, false, false);
            GL.StencilMask(0xff);



            PopMatrix();
        }

        public void RenderWinStencil(IWin win)
        {
            GL.Disable(EnableCap.DepthTest);

            //GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.StencilTest);

            var layer = 0;

            RenderWinStencilEx(win, Vector2.Zero, ref layer);

            Dumper.DumpWhenRequested(@"D:\Projects\Output\OpenBreed\Dumps", "dmp");


            GL.Disable(EnableCap.StencilTest);
        }

        private void RenderWinStencilOrg(IWin win, Vector2 origin, int layer)
        {
            PushMatrix();

            Translate(win.Pos);

            var newPos = origin + win.Pos;

            var stencilFunction = layer > 0 ? StencilFunction.Lequal : StencilFunction.Always;

            GL.StencilFunc(stencilFunction, layer, 0xff);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
            DrawBoxMask(Vector2.Zero, win.Body.Inflated(win.Margin), layer);
            GL.StencilMask(0x00);
            GL.ColorMask(true, true, true, true);
            DrawBox(Vector2.Zero, win.Body, win.BorderColor, win.ForegroundColor);
            GL.ColorMask(false, false, false, false);

            GL.StencilMask(0xff);

            layer++;

            for (int i = 0; i < win.Childs.Count; i++)
            {
                var child = win.Childs[i];

                RenderWinStencilOrg(child, newPos, layer);
            }

            PopMatrix();
        }

        private void RenderWinStencli(IWin win, Vector2 origin, ref int layer)
        {
            if (layer++ == 0)
            {
                GL.Enable(EnableCap.StencilTest);
            }

            PushMatrix();

            Translate(win.Pos);

            var newPos = origin + win.Pos;

            GL.ColorMask(false, false, false, false);
            GL.DepthMask(false);
            GL.StencilFunc(StencilFunction.Always, layer, layer);
            GL.StencilOp(StencilOp.Incr, StencilOp.Incr, StencilOp.Incr);

            DrawBoxMask(Vector2.Zero, win.Body.Inflated(win.Margin), 0);

            GL.ColorMask(true, true, true, true);
            GL.DepthMask(true);
            GL.StencilFunc(StencilFunction.Equal, layer, layer);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);

            // Draw rectangle
            DrawBox(Vector2.Zero, win.Body, win.BorderColor, win.ForegroundColor);

            for (int i = 0; i < win.Childs.Count; i++)
            {
                var child = win.Childs[i];

                RenderWinStencli(child, newPos, ref layer);
            }

            GL.ColorMask(false, false, false, false);
            GL.DepthMask(false);
            GL.StencilFunc(StencilFunction.Always, layer, layer);
            GL.StencilOp(StencilOp.Decr, StencilOp.Decr, StencilOp.Decr);

            DrawBoxMask(Vector2.Zero, win.Body.Inflated(win.Margin), 0);

            GL.ColorMask(true, true, true, true);
            GL.DepthMask(true);

            if (--layer == 0)
                GL.Disable(EnableCap.StencilTest);


            PopMatrix();
        }


        private void RenderWinStencilEx(IWin win, Vector2 origin, ref int layer)
        {
            layer++;

            PushMatrix();

            Translate(win.Pos);

            var newPos = origin + win.Pos;

            // Draw rectangle
            DrawBox(Vector2.Zero, win.Body, win.BorderColor, win.ForegroundColor);


            GL.ColorMask(false, false, false, false);
            GL.DepthMask(false);
            GL.StencilFunc(StencilFunction.Always, layer, layer);
            GL.StencilOp(StencilOp.Incr, StencilOp.Incr, StencilOp.Incr);

            DrawBoxMask(Vector2.Zero, win.Body.Inflated(win.Margin), 0);

            GL.ColorMask(true, true, true, true);
            GL.DepthMask(true);
            GL.StencilFunc(StencilFunction.Equal, layer, layer);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);

            for (int i = 0; i < win.Childs.Count; i++)
            {
                var child = win.Childs[i];

                RenderWinStencilEx(child, newPos, ref layer);
            }

            GL.ColorMask(false, false, false, false);
            GL.DepthMask(false);
            GL.StencilFunc(StencilFunction.Always, layer, layer);
            GL.StencilOp(StencilOp.Decr, StencilOp.Decr, StencilOp.Decr);

            DrawBoxMask(Vector2.Zero, win.Body.Inflated(win.Margin), 0);

            GL.ColorMask(true, true, true, true);
            GL.DepthMask(true);

            PopMatrix();

            layer--;

        }

        private void DrawBoxMask(Vector2 pos, Box2 box, int depth)
        {
            PushMatrix();
            Translate(pos.X, pos.Y, depth);

            //Write to depth buffer
            Context.Primitives.DrawBox(this, box, Color4.White);

            PopMatrix();
        }

        private void DrawBox(Vector2 pos, Box2 box, Color4 borderColor, Color4 foregroundColor)
        {
            var boxMargin = new Vector2(-5, -5);

            PushMatrix();
            Translate(pos.X, pos.Y, 0.0f);
            Context.Primitives.DrawBox(this, box, borderColor);
            Context.Primitives.DrawBox(this, box.Inflated(boxMargin), foregroundColor);

            PopMatrix();
        }

        #endregion Internal Methods
    }

    public class Win : IWin
    {
        public Win(Box2 body, Vector2 margin, Color4 foreColor, Color4 backColor)
        {
            Body = body;
            Margin = margin;
            BorderColor = foreColor;
            ForegroundColor = backColor;
        }

        public Box2 Body { get; }

        public Vector2 Margin { get; }

        public Color4 BorderColor { get; }

        public Color4 ForegroundColor { get; }

        public Vector2 Pos { get; set; }

        public IList<IWin> Childs { get; } = new List<IWin>();
    }

}
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Common;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Rendering.Interface.Extensions;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Rendering.Interface.Factories;

namespace OpenBreed.Editor.UI.Mvc.Views
{
    public class EditorView
    {
        #region Protected Fields

        protected readonly IRenderView renderView;

        #endregion Protected Fields

        #region Private Fields

        private readonly IEventsMan eventsMan;
        private IRenderView cursorView;
        private bool cursorScroll;

        #endregion Private Fields

        #region Public Constructors

        public EditorView(IEventsMan eventsMan, IRenderContext renderContext)
        {
            this.eventsMan = eventsMan;
            renderView = renderContext.CreateView(OnRenderPrivate, 0.0f, 0.0f, 1.0f, 1.0f);

            eventsMan.SubscribeToView<ViewCursorMoveEvent>(renderView, OnCursorMove);
            eventsMan.SubscribeToView<ViewCursorDownEvent>(renderView, OnCursorDown);
            eventsMan.SubscribeToView<ViewCursorUpEvent>(renderView, OnCursorUp);
            eventsMan.SubscribeToView<ViewCursorWheelEvent>(renderView, OnCursorWheel);
        }

        #endregion Public Constructors

        #region Public Events

        public event Action<IRenderView, Matrix4, float> Rendering;

        public event Action<IRenderView> Reseting;

        public event Action<ViewCursorDownEvent> CursorDown;

        #endregion Public Events

        #region Public Properties

        public Vector2i CursorPosition { get; private set; }

        public Vector2i CursorDelta { get; private set; }

        public float MinScale { get; private set; } = 0.125f;

        public float MaxScale { get; private set; } = 8.0f;

        public void SetScaleLimits(float min, float max)
        {
            if (min <= 0.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "Minimum scale must be greater than zero."); 
            }

            if (max <= min)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "Maximum scale must be greater than minimum scale.");
            }

            MinScale = min;
            MaxScale = max;
        }

        #endregion Public Properties

        #region Internal Methods

        internal void Reset()
        {
            OnReset(renderView);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected virtual void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKeys.Middle)
            {
                cursorScroll = true;
            }

            CursorDown?.Invoke(e);
        }

        protected void OnReset(IRenderView view)
        {
            Reseting?.Invoke(view);
        }

        protected virtual void OnCursorUp(ViewCursorUpEvent e)
        {
            if (e.Key == CursorKeys.Middle)
            {
                cursorScroll = false;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnRenderPrivate(IRenderView view, Matrix4 transform, float dt)
        {
            Rendering.Invoke(view, transform, dt);

            if (cursorView == view)
            {
                DrawCursor(view, dt);
            }
        }

        private void OnCursorMove(ViewCursorMoveEvent e)
        {
            cursorView = e.View;

            CursorDelta = e.Position - CursorPosition;
            CursorPosition = e.Position;

            if (cursorScroll)
            {
                renderView.MoveBy(CursorDelta);
            }
        }

        private void OnCursorWheel(ViewCursorWheelEvent e)
        {
            var delta = e.WheelDelta;

            float currentScale = renderView.GetScale();
            float scaleFactor = 1.0f;

            if (Math.Sign(delta) > 0)
            {
                scaleFactor = 2.0f;
            }
            else if (Math.Sign(delta) < 0)
            {
                scaleFactor = 0.5f;
            }

            currentScale *= scaleFactor;

            if (currentScale < MinScale)
            {
                currentScale = MinScale;
            }
            else if (currentScale > MaxScale)
            {
                currentScale = MaxScale;
            }

            renderView.ZoomTo(CursorPosition, currentScale);
        }

        private void DrawCursor(IRenderView view, float dt)
        {
            var wPos = view.GetViewToWorldCoords(CursorPosition);

            //var scale = view.GetScale();
            //view.Scale(1.0f / scale);

            //var cPos = view.GetViewToWorldCoords(CursorPosition);

            var cSize = 20;
            view.Context.Primitives.DrawPoint(view, new Vector2(wPos.X, wPos.Y), Color4.White, PointType.Cross, cSize, ignoreScale: true);
            view.Context.Fonts.Render(view, new Box2(view.Box.Min, view.Box.Max), dt, (view, clipBox, dt) =>  RenderCoordinates(view, clipBox, dt, wPos));
        }

        private void RenderCoordinates(IRenderView view, Box2 clipBox, float dt, Vector4 wPos)
        {
            var textPos = view.GetViewToWorldCoords(CursorPosition);

            var font = view.Context.Fonts.GetOSFont("ARIAL", 9);

            var scale = view.GetScale();


            view.Context.Fonts.RenderStart(view, new Vector2(wPos.X + 5 / scale, wPos.Y + 5 / scale));
            view.Context.Fonts.RenderPart(view, font.Id, $"({wPos.X},{wPos.Y})", Vector2.Zero, Color4.White, 100, clipBox, ignoreScale: true);
            view.Context.Fonts.RenderEnd(view);
        }

        #endregion Private Methods
    }
}
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
using OpenBreed.Editor.VM.Extensions;

namespace OpenBreed.Editor.VM.Base
{
    public abstract class RenderViewControlBase<TModel> : RenderViewControlBase
    {
        #region Protected Constructors

        protected RenderViewControlBase(IEventsMan eventsMan, IRenderContext renderContext) : base(eventsMan, renderContext)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public abstract void Initialize(TModel model);

        public abstract void Save(TModel model);

        #endregion Public Methods
    }

    public abstract class RenderViewControlBase
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;

        private readonly IRenderView renderView;
        private Vector2i cursorPos;
        private Vector2i cursorDelta;
        private IRenderView cursorView;
        private bool cursorScroll;
        private IRenderContext renderContext;

        #endregion Private Fields

        #region Public Constructors

        public RenderViewControlBase(IEventsMan eventsMan, IRenderContext renderContext)
        {
            this.eventsMan = eventsMan;
            this.renderContext = renderContext;
            this.renderView = renderContext.CreateView(OnRenderPrivate, 0.0f, 0.0f, 1.0f, 1.0f);

            eventsMan.SubscribeToView<ViewCursorMoveEvent>(renderView, OnCursorMove);
            eventsMan.SubscribeToView<ViewCursorDownEvent>(renderView, OnCursorDown);
            eventsMan.SubscribeToView<ViewCursorUpEvent>(renderView, OnCursorUp);
        }

        #endregion Public Constructors

        #region Protected Methods

        protected abstract void OnRender(IRenderView view, Matrix4 transform, float dt);

        #endregion Protected Methods

        #region Private Methods

        private void OnCursorMove(ViewCursorMoveEvent e)
        {
            cursorView = e.View;

            cursorDelta = e.Position - cursorPos;
            cursorPos = e.Position;

            if (cursorScroll)
            {
                renderView.View *= Matrix4.CreateTranslation(cursorDelta.X, cursorDelta.Y, 0.0f);
            }
        }

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKeys.Right)
            {
                cursorScroll = true;
            }
        }

        private void OnCursorUp(ViewCursorUpEvent e)
        {
            if (e.Key == CursorKeys.Right)
            {
                cursorScroll = false;
            }
        }

        private void DrawCursor(IRenderView view, float dt)
        {
            var cPos = view.GetViewToWorldCoords(cursorPos);
            var cSize = 10;
            view.Context.Primitives.DrawCircle(view, new Vector2(cPos.X, cPos.Y), cSize, Color4.Red, filled: false);
            view.Context.Primitives.DrawPoint(view, new Vector2(cPos.X, cPos.Y), Color4.Red, PointType.Cross, cSize);
            view.Context.Fonts.Render(view, new Box2(view.Box.Min, view.Box.Max), dt, RenderTexts);
        }

        private void OnRenderPrivate(IRenderView view, Matrix4 transform, float dt)
        {
            if (cursorView == view)
                DrawCursor(view, dt);

            OnRender(view, transform, dt);
        }

        private void RenderTexts(IRenderView view, Box2 clipBox, float dt)
        {
            var textPos = view.GetViewToWorldCoords(cursorPos);

            var font = view.Context.Fonts.GetOSFont("ARIAL", 9);

            view.Context.Fonts.RenderStart(view, new Vector2(textPos.X + 5, textPos.Y + 5));
            view.Context.Fonts.RenderPart(view, font.Id, $"({textPos.X},{textPos.Y})", Vector2.Zero, Color4.Green, 100, clipBox);
            view.Context.Fonts.RenderEnd(view);
        }

        #endregion Private Methods
    }
}
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Abstractions.Helpers;
using OpenBreed.Gui.Extensions;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Views
{
    public abstract class EditorView
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;
        private readonly IInteractionFactoryProvider guiFactoryProvider;
        private bool cursorScroll;

        private IRenderView renderView;

        #endregion Private Fields

        #region Protected Constructors

        protected EditorView(IEventsMan eventsMan, IInteractionFactoryProvider guiFactoryProvider)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.guiFactoryProvider = guiFactoryProvider ?? throw new ArgumentNullException(nameof(guiFactoryProvider));
        }

        #endregion Protected Constructors

        #region Public Events

        public event Action<IRenderView, float> Rendering;

        public event Action<ViewCursorDownEvent> CursorDown;
        public event Action<ViewCursorUpEvent> CursorUp;
        public event Action<ViewCursorMoveEvent> CursorMove;
        public event Action<ViewKeyDownEvent> KeyDown;
        public event Action<ViewKeyUpEvent> KeyUp;

        #endregion Public Events

        #region Public Properties

        public IRenderView RenderView
        {
            get
            {
                return renderView;
            }

            set
            {
                if (renderView == value)
                {
                    return;
                }

                if (renderView is not null)
                {
                    UnregisterRenderView(renderView);
                }

                renderView = value;

                RegisterRenderView(renderView);
                InitGui(renderView);
            }
        }

        public Vector2i CursorPosition { get; private set; }

        public Vector2 CursorInteractionSnapPosition { get; private set; }
        public Vector2 CursorInteractionPosition { get; private set; }
        public Vector2 CursorInteractionDelta { get; private set; }
        public Vector2 CursorInteractionSnapDelta { get; private set; }
        public Vector2i CursorDelta { get; private set; }

        public float MinScale { get; private set; } = 0.0625f;

        public float MaxScale { get; private set; } = 16.0f;

        #endregion Public Properties

        #region Public Methods

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

        public virtual void Reset()
        {
            renderView?.Reset();
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKey.Middle)
            {
                cursorScroll = true;
            }

            CursorDown?.Invoke(e);
        }

        protected virtual void OnCursorUp(ViewCursorUpEvent e)
        {
            if (e.Key == CursorKey.Middle)
            {
                cursorScroll = false;
            }

            CursorUp?.Invoke(e);
        }

        protected virtual void OnKeyDown(ViewKeyDownEvent e)
        {
            KeyDown?.Invoke(e);
        }

        protected virtual void OnKeyUp(ViewKeyUpEvent e)
        {
            KeyUp?.Invoke(e);
        }

        protected virtual void OnRender(IRenderView view, float dt)
        {
        }

        protected virtual Vector2 GetInteractionSnapCursorPosition(Vector2 coordinates)
        {
            return coordinates;
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitGui(IRenderView renderView)
        {
            var guiFactory = guiFactoryProvider.GetFactory(renderView);

            var desktop = guiFactory.CreateDesktop(builder =>
            {
                //CreateButtonCtrlTest(builder);

                //CreateCheckboxCtrlTest(builder);

                //CreateLabelCtrlTest(builder);
            });

            desktop.AddChild(guiFactory.CreateScrollbar((builder) =>
            {
                builder.SetMovable(false);

                builder.SetMode(Gui.Abstractions.Constants.ScrollbarMode.Horizontal);
                builder.SetValue(-100.0f);
                builder.SetMaximumSize(float.MaxValue, 16.0f);
                builder.SetMinimumValue(-100.0f);
                builder.SetMaximumValue(200.0f);
                builder.SetValueUnit(25.0f);
                builder.SetDockMode(ElementDockMode.Bottom);
                //builder.BindValue(PropertyBinding<float>.Create(data, (obj) => obj.ScrollTestVertical));
                //builder.SetGridPosition(2, 1);
            }));

            desktop.AddChild(guiFactory.CreateScrollbar((builder) =>
            {
                builder.SetMovable(false);

                builder.SetMode(Gui.Abstractions.Constants.ScrollbarMode.Vertical);
                builder.SetMaximumSize(16.0f, float.MaxValue);
                builder.SetValue(-100.0f);
                builder.SetMinimumValue(-100.0f);
                builder.SetMaximumValue(200.0f);
                builder.SetValueUnit(25.0f);
                builder.SetDockMode(ElementDockMode.Right);
                //builder.BindValue(PropertyBinding<float>.Create(data, (obj) => obj.ScrollTestVertical));
                //builder.SetGridPosition(2, 1);
            }));
        }

        private void UnregisterRenderView(IRenderView renderView)
        {
            eventsMan.Unsubscribe<ViewCursorMoveEvent>(OnCursorMove);
            eventsMan.Unsubscribe<ViewCursorDownEvent>(OnCursorDown);
            eventsMan.Unsubscribe<ViewCursorUpEvent>(OnCursorUp);
            eventsMan.Unsubscribe<ViewCursorWheelEvent>(OnCursorWheel);
            eventsMan.Unsubscribe<ViewKeyDownEvent>(OnKeyDown);
            eventsMan.Unsubscribe<ViewKeyUpEvent>(OnKeyUp);

            renderView.Rendering -= OnRenderPrivate;
        }

        private void RegisterRenderView(IRenderView renderView)
        {
            renderView.Rendering += OnRenderPrivate;

            eventsMan.SubscribeToView<ViewCursorMoveEvent>(renderView, OnCursorMove);
            eventsMan.SubscribeToView<ViewCursorDownEvent>(renderView, OnCursorDown);
            eventsMan.SubscribeToView<ViewCursorUpEvent>(renderView, OnCursorUp);
            eventsMan.SubscribeToView<ViewCursorWheelEvent>(renderView, OnCursorWheel);
            eventsMan.SubscribeToView<ViewKeyDownEvent>(renderView, OnKeyDown);
            eventsMan.SubscribeToView<ViewKeyUpEvent>(renderView, OnKeyUp);
        }

        private void OnRenderPrivate(IRenderView view, float dt)
        {
            view.PushMatrix();

            try
            {
                OnRender(view, dt);
                Rendering?.Invoke(view, dt);

                RenderCursor(view, view.Box.AsBox2());
            }
            finally
            {
                view.PopMatrix();
            }
        }

        private void OnCursorMove(ViewCursorMoveEvent e)
        {
            CursorDelta = e.Position - CursorPosition;
            CursorPosition = e.Position;

            var newInteractionPosition = GetInteractionCursorPosition(e.View);
            var newInteractionSnapPosition = GetInteractionSnapCursorPosition(newInteractionPosition);
            CursorInteractionDelta = newInteractionPosition - CursorInteractionPosition;
            CursorInteractionPosition = newInteractionPosition;
            CursorInteractionSnapDelta = newInteractionSnapPosition - CursorInteractionSnapPosition;
            CursorInteractionSnapPosition = newInteractionSnapPosition;



            if (cursorScroll)
            {
                renderView.MoveBy(CursorDelta);
            }

            CursorMove?.Invoke(e);
        }

        private void OnCursorWheel(ViewCursorWheelEvent e)
        {
            var delta = e.WheelDelta;

            float currentScale = renderView.GetScaleX();
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

        protected virtual Vector2 GetInteractionCursorPosition(IRenderView view)
        {
            var position4 = view.ToWorldPoint(CursorPosition);
            return new Vector2(position4.X, position4.Y);
        }

        private void RenderCursor(IRenderView view, Box2 clipBox)
        {
            var cursorPos = CursorInteractionSnapPosition;

            var fontMan = view.Context.ServiceProvider.GetRequiredService<IFontMan>();
            var font = fontMan.GetOSFont("ARIAL", 9);
            var scale = view.GetScale();

            view.Context.Primitives.DrawPoint(view, cursorPos, Color4.Green, OpenBreed.Rendering.Abstractions.Renderers.PointType.Cross, size: 20, ignoreScale: true);

            view.Context.FontRenderer.RenderStart(view, new Vector2(cursorPos.X + 5 / scale.X, cursorPos.Y + 5 / scale.Y));
            view.Context.FontRenderer.RenderPart(view, font.Id, $"({cursorPos.X},{cursorPos.Y})", Vector2.Zero, Color4.White, 100, clipBox, ignoreScale: true);
            view.Context.FontRenderer.RenderEnd(view);
        }

        #endregion Private Methods
    }
}
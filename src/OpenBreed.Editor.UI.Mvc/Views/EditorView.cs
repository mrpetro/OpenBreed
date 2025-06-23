using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
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
    public class EditorView
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;
        private readonly IInteractionFactoryProvider guiFactoryProvider;
        private IRenderView renderView;
        private bool cursorScroll;

        private IRenderContext renderContext;

        #endregion Private Fields

        #region Public Constructors

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

        public EditorView(IEventsMan eventsMan, IInteractionFactoryProvider guiFactoryProvider)
        {
            this.eventsMan = eventsMan;
            this.guiFactoryProvider = guiFactoryProvider;
        }

        #endregion Public Constructors

        #region Public Events

        public event Action<IRenderView, float> Rendering;

        public event Action<ViewCursorDownEvent> CursorDown;

        #endregion Public Events

        #region Public Properties

        public IRenderContext RenderContext
        {
            get
            {
                return renderContext;
            }

            set
            {
                if (renderContext == value)
                {
                    return;
                }

                if (renderContext is not null)
                {
                    UnregisterRenderView(renderView);
                    renderContext.RemoveView(renderView);
                }

                renderContext = value;
                renderView = renderContext.CreateView(0.0f, 0.0f, 1.0f, 1.0f);
                RegisterRenderView(renderView);
                InitGui(renderView);
            }
        }

        public Vector2i CursorPosition { get; private set; }

        public Vector2i CursorDelta { get; private set; }

        public float MinScale { get; private set; } = 0.125f;

        public float MaxScale { get; private set; } = 8.0f;

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

        #endregion Public Methods

        #region Internal Methods

        internal void Reset()
        {
            renderView?.Reset();
        }

        #endregion Internal Methods

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
        }

        #endregion Protected Methods

        #region Private Methods

        private void UnregisterRenderView(IRenderView renderView)
        {
            eventsMan.Unsubscribe<ViewCursorMoveEvent>(OnCursorMove);
            eventsMan.Unsubscribe<ViewCursorDownEvent>(OnCursorDown);
            eventsMan.Unsubscribe<ViewCursorUpEvent>(OnCursorUp);
            eventsMan.Unsubscribe<ViewCursorWheelEvent>(OnCursorWheel);

            renderView.Rendering -= OnRenderPrivate;
        }

        private void RegisterRenderView(IRenderView renderView)
        {
            renderView.Rendering += OnRenderPrivate;

            eventsMan.SubscribeToView<ViewCursorMoveEvent>(renderView, OnCursorMove);
            eventsMan.SubscribeToView<ViewCursorDownEvent>(renderView, OnCursorDown);
            eventsMan.SubscribeToView<ViewCursorUpEvent>(renderView, OnCursorUp);
            eventsMan.SubscribeToView<ViewCursorWheelEvent>(renderView, OnCursorWheel);
        }

        private void OnRenderPrivate(IRenderView view, float dt)
        {
            view.PushMatrix();

            try
            {
                Rendering?.Invoke(view, dt);

                RenderCoordinates(view, view.Box.AsBox2());
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

        private void RenderCoordinates(IRenderView view, Box2 clipBox)
        {
            var textPos = view.ToWorldPoint(CursorPosition);

            var font = view.Context.Fonts.GetOSFont("ARIAL", 9);

            var scale = view.GetScale();

            view.Context.Fonts.RenderStart(view, new Vector2(textPos.X + 5 / scale, textPos.Y + 5 / scale));
            view.Context.Fonts.RenderPart(view, font.Id, $"({textPos.X},{textPos.Y})", Vector2.Zero, Color4.White, 100, clipBox, ignoreScale: true);
            view.Context.Fonts.RenderEnd(view);
        }

        #endregion Private Methods
    }
}
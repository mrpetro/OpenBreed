using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Common;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Extensions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;

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

        public EditorView(IEventsMan eventsMan, IInteractionFactoryProvider guiFactory, IRenderContext renderContext)
        {
            this.eventsMan = eventsMan;
            renderView = renderContext.CreateView(0.0f, 0.0f, 1.0f, 1.0f);


            GuiFactory = guiFactory.GetFactory(renderView);

            var desktop = GuiFactory.CreateDesktop(builder =>
            {


                //CreateButtonCtrlTest(builder);

                //CreateCheckboxCtrlTest(builder);

                //CreateLabelCtrlTest(builder);

            });

            renderView.Rendering += OnRenderPrivate;

            eventsMan.SubscribeToView<ViewCursorMoveEvent>(renderView, OnCursorMove);
            eventsMan.SubscribeToView<ViewCursorDownEvent>(renderView, OnCursorDown);
            eventsMan.SubscribeToView<ViewCursorUpEvent>(renderView, OnCursorUp);
            eventsMan.SubscribeToView<ViewCursorWheelEvent>(renderView, OnCursorWheel);

            desktop.AddChild(GuiFactory.CreateScrollbar((builder) =>
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

            desktop.AddChild(GuiFactory.CreateScrollbar((builder) =>
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

        #endregion Public Constructors

        #region Public Events

        public event Action<IRenderView, Matrix4, float> Rendering;

        public event Action<ViewCursorDownEvent> CursorDown;

        #endregion Public Events

        #region Public Properties

        public Vector2i CursorPosition { get; private set; }

        public Vector2i CursorDelta { get; private set; }

        public float MinScale { get; private set; } = 0.125f;

        public float MaxScale { get; private set; } = 8.0f;
        public IInteractionFactory GuiFactory { get; }

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
            renderView.Reset();
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

        private void OnRenderPrivate(IRenderView view, Matrix4 transform, float dt)
        {
            view.PushMatrix();

            try
            {
                Rendering.Invoke(view, transform, dt);
            }
            finally
            {
                view.PopMatrix();
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

        private void RenderCoordinates(IRenderView view, Box2 clipBox, Vector4 wPos)
        {
            var textPos = view.ToWorldPoint(CursorPosition);

            var font = view.Context.Fonts.GetOSFont("ARIAL", 9);

            var scale = view.GetScale();


            view.Context.Fonts.RenderStart(view, new Vector2(wPos.X + 5 / scale, wPos.Y + 5 / scale));
            view.Context.Fonts.RenderPart(view, font.Id, $"({wPos.X},{wPos.Y})", Vector2.Zero, Color4.White, 100, clipBox, ignoreScale: true);
            view.Context.Fonts.RenderEnd(view);
        }

        #endregion Private Methods
    }
}
using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OpenTK.Windowing.Common;
using System.Net;
using OpenBreed.Input.Interface;
using System.Windows.Controls;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Events;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Gui.Interface;
using System.Diagnostics;
using OpenBreed.Gui.Interface.Rendering;

namespace RendererTest.Wpf.App.VM
{
    public class RendererVm : BaseViewModel
    {
        private readonly IEventsMan eventsMan;
        private readonly IInteractionCore interactionCore;
        #region Private Fields

        private readonly Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> renderContextProvider;
        private Vector2i cursorPos;
        private Vector2i cursorDelta;
        private IRenderView cursorView;
        private bool cursorScroll;

        private IRenderContext renderContext;
        private IRenderView renderView;
        private IRenderView renderView2;

        #endregion Private Fields

        #region Public Constructors

        public RendererVm(IEventsMan eventsMan, 
            IInteractionCore interactionCore,
            Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> renderContextProvider)
        {
            this.eventsMan = eventsMan;
            this.interactionCore = interactionCore;
            this.renderContextProvider = renderContextProvider;

            InitFunc = OnInitialize;

            eventsMan.Subscribe<ViewCursorMoveEvent>(OnCursorMove);
            eventsMan.Subscribe<ViewCursorDownEvent>(OnCursorDown);
            eventsMan.Subscribe<ViewCursorUpEvent>(OnCursorUp);
            eventsMan.Subscribe<ViewCursorEnterEvent>(OnCursorEnter);
            eventsMan.Subscribe<ViewCursorLeaveEvent>(OnCursorLeave);
            eventsMan.Subscribe<ViewCursorWheelEvent>(OnCursorWheel);





        }

        #endregion Public Constructors

        #region Public Properties

        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc { get; }

        #endregion Public Properties

        #region Private Methods

        private IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            renderContext = renderContextProvider.Invoke(graphicsContext, hostCoordinateSystemConverter);

            renderView = renderContext.CreateView(OnRender1, 0.0f, 0.0f, 1.0f, 1.0f);


            var element = interactionCore
                .BeginLabel()
                    .SetTag("Form")
                    .SetPosition(200.0f, 200.0f)
                    .SetSize(200, 100)
                    .BeginLabel()
                        .SetTag("Ok")
                        .SetPosition(-75.0f, 0.0f)
                        .SetSize(25, 25)
                        .SetClickCallback(ButtonClicked)
                        .FinishElement()
                    .BeginLabel()
                        .SetTag("Cancel")
                        .SetPosition(75.0f, 0.0f)
                        .SetSize(25, 25)
                        .SetClickCallback(ButtonClicked)
                        .FinishElement()
                .Build();


            interactionCore.Root = element;

            return renderContext;
        }

        private object SetPosition(float v1, float v2)
        {
            throw new NotImplementedException();
        }

        private void ButtonClicked(IInteractiveElement interactiveElement)
        {
            Debug.WriteLine($"Interactive element '{interactiveElement.Tag}' clicked.");
        }

        private static OpenBreed.Gui.Interface.CursorKey ToCKey(CursorKeys key)
        {
            switch (key)
            {
                case CursorKeys.Left:
                    return CursorKey.Left;
                case CursorKeys.Middle:
                    return CursorKey.Middle;
                case CursorKeys.Right:
                    return CursorKey.Right;
                case CursorKeys.XButton1:
                case CursorKeys.XButton2:
                default:
                    throw new NotImplementedException();
            }
        }

        private void OnCursorMove(ViewCursorMoveEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            cursorView = e.View;

            cursorDelta = e.Position - cursorPos;
            cursorPos = e.Position;

            if (cursorScroll)
            {
                e.View.View *= Matrix4.CreateTranslation(cursorDelta.X, cursorDelta.Y, 0.0f);
            }

            var cPos = e.View.GetViewToWorldCoords(cursorPos);
            interactionCore.Move(e.CursorId, cPos.X, cPos.Y);
        }

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            var cPos = e.View.GetViewToWorldCoords(cursorPos);
            interactionCore.Down(e.CursorId, cPos.X, cPos.Y, ToCKey(e.Key));

            if (e.Key == CursorKeys.Right)
            {
                cursorScroll = true;
            }
        }

        private void OnCursorEnter(ViewCursorEnterEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            var cPos = e.View.GetViewToWorldCoords(cursorPos);
            interactionCore.Enter(e.CursorId, cPos.X, cPos.Y);

        }

        private void OnCursorWheel(ViewCursorWheelEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            var cPos = e.View.GetViewToWorldCoords(cursorPos);
            interactionCore.Wheel(e.CursorId, cPos.X, cPos.Y, e.WheelDelta);
        }

        private void OnCursorLeave(ViewCursorLeaveEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            interactionCore.Leave(e.CursorId);
        }

        private void OnCursorUp(ViewCursorUpEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            var cPos = e.View.GetViewToWorldCoords(cursorPos);
            interactionCore.Up(e.CursorId, cPos.X, cPos.Y, ToCKey(e.Key));

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

        private void OnRender1(IRenderView view, Matrix4 transform, float dt)
        {
            if (interactionCore.Root is not null)
            {
                var interactionRederer = new InteractionRenderer();
                interactionRederer.Render(interactionCore.Root, view);
            }

            if (cursorView == view)
                DrawCursor(view, dt);
        }


        private void RenderTexts(IRenderView view, Box2 clipBox, float dt)
        {
            var cPos = view.GetViewToWorldCoords(cursorPos);

            var font = view.Context.Fonts.GetOSFont("ARIAL", 12);

            view.Context.Fonts.RenderStart(view, new Vector2(cPos.X, cPos.Y));
            view.Context.Fonts.RenderPart(view, font.Id, $"({cPos.X},{cPos.Y})", Vector2.Zero, Color4.Green, 100, clipBox);
            view.Context.Fonts.RenderEnd(view);
        }

        #endregion Private Methods
    }
}
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

namespace RendererTest.Wpf.App.VM
{
    public class RendererVm : BaseViewModel
    {
        private readonly IEventsMan eventsMan;
        #region Private Fields

        private readonly Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> renderContextProvider;
        private Vector2i cursorPos;
        private Vector2i cursorDelta;
        private IRenderView cursorView;
        private bool cursorScroll;

        private IRenderContext renderContext;
        private IRenderView renderView1;
        private IRenderView renderView2;

        #endregion Private Fields

        #region Public Constructors

        public RendererVm(IEventsMan eventsMan, 
            Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> renderContextProvider)
        {
            this.eventsMan = eventsMan;
            this.renderContextProvider = renderContextProvider;

            InitFunc = OnInitialize;

            eventsMan.Subscribe<ViewCursorMoveEvent>(OnCursorMove);
            eventsMan.Subscribe<ViewCursorDownEvent>(OnCursorDown);
            eventsMan.Subscribe<ViewCursorUpEvent>(OnCursorUp);
        }

        #endregion Public Constructors

        #region Public Properties

        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc { get; }

        #endregion Public Properties

        #region Private Methods

        private IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            renderContext = renderContextProvider.Invoke(graphicsContext, hostCoordinateSystemConverter);

            renderView1 = renderContext.CreateView(OnRender1, 0.0f, 0.0f, 0.5f, 1.0f);
            renderView2 = renderContext.CreateView(OnRender2, 0.5f, 0.0f, 1.0f, 1.0f);

            return renderContext;
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
        }

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            if (e.Key ==CursorKeys.Right)
            {
                cursorScroll = true;
            }
        }

        private void OnCursorUp(ViewCursorUpEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

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
            if (cursorView == view)
                DrawCursor(view, dt);

            view.Context.Primitives.DrawRectangle(view, new Box2(0, 0, 20, 30), Color4.Red, filled: true);
        }

        private void OnRender2(IRenderView view, Matrix4 transform, float dt)
        {
            if (cursorView == view)
                DrawCursor(view, dt);

            view.Context.Primitives.DrawRectangle(view, new Box2(0, 0, 200, 30), Color4.Blue, filled: true);
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
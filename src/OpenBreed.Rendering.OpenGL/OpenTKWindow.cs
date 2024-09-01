using Microsoft.Extensions.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;

namespace OpenBreed.Rendering.OpenGL
{
    internal class OpenTKWindow : IWindow
    {
        #region Private Fields

        private readonly GameWindow gameWindow;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;
        private readonly IRenderContextFactory renderContextFactory;

        #endregion Private Fields

        #region Public Constructors

        public OpenTKWindow(
            IEventsMan eventsMan,
            ILogger logger,
            IRenderContextFactory renderContextFactory,
            GameWindow gameWindow)
        {
            this.eventsMan = eventsMan;
            this.logger = logger;
            this.renderContextFactory = renderContextFactory;
            this.gameWindow = gameWindow;

            this.gameWindow.Load += Window_Load;
            this.gameWindow.Resize += Window_Resize;
            this.gameWindow.UpdateFrame += Window_UpdateFrame;
            this.gameWindow.RenderFrame += Window_RenderFrame;
            this.gameWindow.MouseDown += GameWindow_MouseUp;
            this.gameWindow.MouseDown += GameWindow_MouseDown;
            this.gameWindow.MouseEnter += GameWindow_MouseEnter;
            this.gameWindow.MouseLeave += GameWindow_MouseLeave;
            this.gameWindow.MouseMove += GameWindow_MouseMove;
            this.gameWindow.MouseWheel += GameWindow_MouseWheel; ;
        }

        #endregion Public Constructors

        #region Public Properties

        public Box2i ClientRectangle => gameWindow.ClientRectangle;

        public IRenderContext Context { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Exit()
        {
            gameWindow.Close();
        }

        public void Run()
        {
            gameWindow.Run();
        }

        #endregion Public Methods

        #region Private Methods

        private void Window_Load()
        {
            Context = new OpenTKRenderContext(logger, eventsMan, gameWindow.Context, GetRenderContextPosition);
            eventsMan.Raise(new WindowLoadEvent(this, Context));
        }

        private void GameWindow_MouseLeave()
        {
            var cursorPosition = gameWindow.MousePosition;
            Context.CursorLeave(0, (Vector2i)cursorPosition);
        }

        private void GameWindow_MouseEnter()
        {
            var cursorPosition = gameWindow.MousePosition;
            Context.CursorEnter(0, (Vector2i)cursorPosition);
        }

        private void GameWindow_MouseUp(MouseButtonEventArgs e)
        {
            var cursorPosition = gameWindow.MousePosition;
            Context.CursorUp(0, (Vector2i)cursorPosition, (CursorKeys)e.Button);
        }

        private void GameWindow_MouseDown(MouseButtonEventArgs e)
        {
            var cursorPosition = gameWindow.MousePosition;
            Context.CursorDown(0, (Vector2i)cursorPosition, (CursorKeys)e.Button);
        }

        private void GameWindow_MouseMove(MouseMoveEventArgs e)
        {
            var cursorPosition = e.Position;
            Context.CursorMove(0, (Vector2i)cursorPosition);
        }

        private void GameWindow_MouseWheel(MouseWheelEventArgs e)
        {
            var cursorPosition = gameWindow.MousePosition;
            Context.CursorWheel(0, (Vector2i)cursorPosition, (int)e.OffsetY);
        }

        private Vector2i GetRenderContextPosition(Vector2i point)
        {
            var pointV = new Vector4(point.X, point.Y, 0.0f, 1.0f);

            var translateTranform = Matrix4.CreateTranslation(0.0f, gameWindow.ClientSize.Y, 0.0f);
            var flipYTransform = Matrix4.CreateScale(1.0f, -1.0f, 1.0f);
  
            var matT = flipYTransform * translateTranform;
            pointV *= matT;

            return new Vector2i((int)pointV.X, (int)pointV.Y);
        }

        private void Window_Resize(ResizeEventArgs obj)
        {
            Context.Resize(obj.Width, obj.Height);

            eventsMan.Raise(new WindowResizeEvent(this, ClientRectangle.Size.X, ClientRectangle.Size.Y));
        }

        private void Window_UpdateFrame(FrameEventArgs e)
        {
            eventsMan.Raise(new WindowUpdateEvent(this, (float)e.Time));
        }

        private void Window_RenderFrame(FrameEventArgs e)
        {
            Context.Render((float)e.Time);

            eventsMan.Raise(new WindowRenderEvent(this, (float)e.Time));

            gameWindow.SwapBuffers();
        }

        #endregion Private Methods
    }
}
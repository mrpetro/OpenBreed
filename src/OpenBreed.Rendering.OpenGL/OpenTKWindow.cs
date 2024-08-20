using Microsoft.Extensions.Logging;
using OpenBreed.Core;
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
using System.Windows;

namespace OpenBreed.Rendering.OpenGL
{
    internal class OpenTKWindow : IWindow
    {
        #region Private Fields

        private readonly GameWindow gameWindow;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public OpenTKWindow(
            IEventsMan eventsMan,
            ILogger logger,
            GameWindow gameWindow)
        {
            this.eventsMan = eventsMan;
            this.logger = logger;
            this.gameWindow = gameWindow;

            this.gameWindow.Load += Window_Load;
            this.gameWindow.Resize += Window_Resize;
            this.gameWindow.UpdateFrame += Window_UpdateFrame;
            this.gameWindow.RenderFrame += Window_RenderFrame;
        }

        #endregion Public Constructors

        #region Public Properties

        public Matrix4 ClientTransform { get; private set; }

        public float ClientRatio
        { get { return ClientRectangle.Size.X / (float)ClientRectangle.Size.Y; } }

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
            Context = new OpenTKRenderContext(logger, eventsMan, gameWindow.Context);
            eventsMan.Raise(new WindowLoadEvent(this));
        }

        private void Window_Resize(ResizeEventArgs obj)
        {
            ClientTransform = Matrix4.Identity;
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateTranslation(0.0f, -ClientRectangle.Size.Y, 0.0f));
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateTranslation(-ClientRectangle.Size.X / 2.0f, ClientRectangle.Size.Y / 2.0f, 0.0f));
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateScale(2.0f / ClientRectangle.Size.X, -2.0f / ClientRectangle.Size.Y, 1.0f));

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
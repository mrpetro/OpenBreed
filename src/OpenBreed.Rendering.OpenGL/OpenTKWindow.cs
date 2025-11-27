using Microsoft.Extensions.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.Abstractions.Managers;
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows;

namespace OpenBreed.Rendering.OpenGL
{
    internal class OpenTKWindow : IWindow
    {
        #region Private Fields

        private static readonly Matrix4 flipYTransform = Matrix4.CreateScale(1.0f, -1.0f, 1.0f);
        private readonly GameWindow gameWindow;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;
        private readonly IPaletteMan paletteMan;
        private readonly IStampMan stampMan;
        private readonly IRenderContextProvider renderContextFactory;

        #endregion Private Fields

        #region Public Constructors

        public OpenTKWindow(
            IEventsMan eventsMan,
            ILogger logger,
            IPaletteMan paletteMan,
            IStampMan stampMan,
            IRenderContextProvider renderContextFactory,
            GameWindow gameWindow)
        {
            this.eventsMan = eventsMan;
            this.logger = logger;
            this.paletteMan = paletteMan;
            this.stampMan = stampMan;
            this.renderContextFactory = renderContextFactory;
            this.gameWindow = gameWindow;

            this.gameWindow.Load += Window_Load;
            this.gameWindow.Resize += Window_Resize;
            this.gameWindow.UpdateFrame += Window_UpdateFrame;
            this.gameWindow.RenderFrame += Window_RenderFrame;
            this.gameWindow.MouseUp += GameWindow_MouseUp;
            this.gameWindow.MouseDown += GameWindow_MouseDown;
            this.gameWindow.MouseEnter += GameWindow_MouseEnter;
            this.gameWindow.MouseLeave += GameWindow_MouseLeave;
            this.gameWindow.MouseMove += GameWindow_MouseMove;
            this.gameWindow.MouseWheel += GameWindow_MouseWheel;
            this.gameWindow.TextInput += GameWindow_TextInput;
            this.gameWindow.KeyDown += GameWindow_KeyDown;
            this.gameWindow.KeyUp += GameWindow_KeyUp;
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
            Context = renderContextFactory.GetContext(gameWindow.Context, GetRenderContextPosition);
        
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
            Context.CursorUp(0, (Vector2i)cursorPosition, (CursorKey)e.Button, (Abstractions.Events.KeyModifiers)e.Modifiers);
        }

        private void GameWindow_MouseDown(MouseButtonEventArgs e)
        {
            var cursorPosition = gameWindow.MousePosition;
            Context.CursorDown(0, (Vector2i)cursorPosition, (CursorKey)e.Button, (Abstractions.Events.KeyModifiers)e.Modifiers);
        }

        private void GameWindow_MouseMove(MouseMoveEventArgs e)
        {
            var cursorKeyStates = GetCursorKeyStates().ToArray();
            var modifiers = GetKeyModifiers();
            var cursorPosition = e.Position;
            Context.CursorMove(0, (Vector2i)cursorPosition, new BitArray(cursorKeyStates), modifiers);
        }

        private Abstractions.Events.KeyModifiers GetKeyModifiers()
        {
            var modifiers = (gameWindow.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightShift) || gameWindow.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightShift)) ? Abstractions.Events.KeyModifiers.Shift : 0;
            modifiers |= (gameWindow.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftControl) || gameWindow.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightControl)) ? Abstractions.Events.KeyModifiers.Control : 0;
            modifiers |= (gameWindow.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftAlt) || gameWindow.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightAlt)) ? Abstractions.Events.KeyModifiers.Alt : 0;
            modifiers |= gameWindow.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.CapsLock) ? Abstractions.Events.KeyModifiers.CapsLock : 0;
            modifiers |= gameWindow.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.NumLock) ? Abstractions.Events.KeyModifiers.NumLock : 0;

            return modifiers;
        }

        private void GameWindow_MouseWheel(MouseWheelEventArgs e)
        {
            var cursorPosition = gameWindow.MousePosition;
            Context.CursorWheel(0, (Vector2i)cursorPosition, (int)e.OffsetY);
        }

        private void GameWindow_KeyDown(KeyboardKeyEventArgs obj)
        {
            Context.KeyDown((Abstractions.Events.Keys)obj.Key, (Abstractions.Events.KeyModifiers)obj.Modifiers);
        }

        private void GameWindow_KeyUp(KeyboardKeyEventArgs obj)
        {
            Context.KeyUp((Abstractions.Events.Keys)obj.Key, (Abstractions.Events.KeyModifiers)obj.Modifiers);
        }

        private void GameWindow_TextInput(TextInputEventArgs obj)
        {
            Context.TextInput(obj.AsString);
        }

        private Vector2i GetRenderContextPosition(Vector2i point)
        {
            var pointV = new Vector4(point.X, point.Y, 0.0f, 1.0f);

            var translateTranform = Matrix4.CreateTranslation(0.0f, gameWindow.ClientSize.Y, 0.0f);
            
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

        private IEnumerable<bool> GetCursorKeyStates()
        {
            yield return gameWindow.IsMouseButtonPressed(MouseButton.Left);
            yield return gameWindow.IsMouseButtonPressed(MouseButton.Middle);
            yield return gameWindow.IsMouseButtonPressed(MouseButton.Right);
            yield return gameWindow.IsMouseButtonPressed(MouseButton.Button1);
            yield return gameWindow.IsMouseButtonPressed(MouseButton.Button2);
        }

        #endregion Private Methods
    }
}
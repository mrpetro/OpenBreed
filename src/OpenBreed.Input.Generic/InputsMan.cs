using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Events;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Abstractions;
using OpenBreed.Input.Abstractions.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Input.Generic
{
    internal class EditorInputsMan : IInputsMan
    {
        #region Public Events

        public event EventHandler<KeyboardStateEventArgs> KeyboardStateChanged;

        #endregion Public Events

        #region Public Properties

        public Vector2 CursorDelta { get; }

        public Vector2 CursorPos { get; }

        public float WheelDelta { get; }

        public float WheelPos { get; }

        public bool IsMousePressed { get; }

        public bool IsLeftMousePressed { get; }

        public bool IsMiddleMousePressed { get; }

        public bool IsRightMousePressed { get; }

        #endregion Public Properties

        #region Public Methods

        public bool IsKeyPressed(int inputCode)
        {
            return false;
        }

        public void Update()
        {
        }

        #endregion Public Methods
    }

    internal class InputsMan : IInputsMan
    {
        #region Private Fields

        private readonly GameWindow gameWindow;

        private readonly IEventsMan eventsMan;
        private readonly IReadOnlyList<Keys> scanKeys;
        private Vector2 oldCursorPos;
        private KeyboardState previousKeyboardState;
        private MouseState oldMouseState;
        private float oldWheelPos;

        private HashSet<Keys> keysPressed = new HashSet<Keys>();

        private HashSet<Keys> keysReleased = new HashSet<Keys>();

        #endregion Private Fields

        #region Public Constructors

        public InputsMan(
            GameWindow gameWindow,
            IEventsMan eventsMan)
        {
            this.gameWindow = gameWindow;
            this.eventsMan = eventsMan;

            //gameWindow.MouseMove += OnMouseMove;
            gameWindow.MouseWheel += OnMouseWheel;
            gameWindow.KeyDown += OnKeyDown;
            gameWindow.KeyUp += OnKeyUp;
            gameWindow.MouseDown += OnMouseDown;
            gameWindow.MouseUp += OnMouseUp;

            gameWindow.Load += GameWindow_Load;
            this.eventsMan.Subscribe<WindowUpdateEvent>((e) => OnUpdateFrame(e.Dt));
            this.scanKeys = GetScanKeys().ToList();

            previousKeyboardState = gameWindow.KeyboardState.GetSnapshot();
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<KeyboardStateEventArgs> KeyboardStateChanged;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets position delta (difference between current and previous)
        /// </summary>
        public Vector2 CursorDelta { get; private set; }

        /// <summary>
        /// Gets cursor position in client coordinates
        /// </summary>
        public Vector2 CursorPos => gameWindow.MousePosition;

        /// <summary>
        /// Gets wheel delta (difference between current and previous)
        /// </summary>
        public float WheelDelta { get; private set; }

        /// <summary>
        /// Gets cursor wheel value
        /// </summary>
        public float WheelPos { get; private set; }

        public bool IsMousePressed => gameWindow.MouseState.IsAnyButtonDown;

        public bool IsLeftMousePressed => gameWindow.MouseState.IsButtonDown(MouseButton.Left);

        public bool IsMiddleMousePressed => gameWindow.MouseState.IsButtonDown(MouseButton.Middle);

        public bool IsRightMousePressed => gameWindow.MouseState.IsButtonDown(MouseButton.Right);

        #endregion Public Properties

        #region Public Methods

        public bool IsKeyPressed(int inputCode)
        {
            return gameWindow.KeyboardState.IsKeyDown((Keys)inputCode);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void OnKeyboardStateChanged(KeyboardState keyboardState)
        {
            var eventArgs = new KeyboardStateEventArgs(keyboardState, keysPressed, keysReleased);
            KeyboardStateChanged?.Invoke(this, eventArgs);
            eventsMan.Raise(eventArgs);
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnUpdateFrame(float dt)
        {
            CheckKeyboardState();

            var newMouseState = gameWindow.MouseState.GetSnapshot();
            try
            {
                CursorDelta = CursorPos - oldCursorPos;
                oldCursorPos = CursorPos;

                WheelDelta = WheelPos - oldWheelPos;
                oldWheelPos = WheelPos;
            }
            finally
            {
                oldMouseState = newMouseState;
            }
        }

        private IEnumerable<Keys> GetScanKeys()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (key == Keys.Unknown)
                {
                    continue;
                }

                yield return key;
            }
        }

        private void CheckKeyboardState()
        {
            keysPressed.Clear();
            keysReleased.Clear();
            var currentKeyboardState = gameWindow.KeyboardState.GetSnapshot();

            try
            {
                foreach (Keys key in scanKeys)
                {
                    if (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
                    {
                        keysPressed.Add(key);
                        //Console.WriteLine($"Key pressed: {key}");
                    }

                    if (!currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key))
                    {
                        keysReleased.Add(key);
                        //Console.WriteLine($"Key released: {key}");
                    }
                }

                if (keysPressed.Count > 0 || keysReleased.Count > 0)
                {
                    OnKeyboardStateChanged(currentKeyboardState);
                }
            }
            finally
            {
                previousKeyboardState = currentKeyboardState;
            }
        }

        private void GameWindow_Load()
        {
            oldCursorPos = CursorPos;
        }

        private void OnKeyDown(KeyboardKeyEventArgs e)
        {
            eventsMan.Raise(new KeyDownEvent());
        }

        private void OnKeyUp(KeyboardKeyEventArgs e)
        {
            eventsMan.Raise(new KeyUpEvent());
        }

        private void OnMouseDown(MouseButtonEventArgs e)
        {
            eventsMan.Raise(new MouseDownEvent());
        }

        private void OnMouseUp(MouseButtonEventArgs e)
        {
            eventsMan.Raise(new MouseUpEvent());
        }

        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            eventsMan.Raise(new MouseWheelEvent());
        }

        #endregion Private Methods
    }
}
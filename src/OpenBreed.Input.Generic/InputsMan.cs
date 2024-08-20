using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Input.Interface.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Input.Generic
{
    internal class InputsMan : IInputsMan
    {
        #region Private Fields

        private readonly GameWindow gameWindow;

        private readonly IEventsMan eventsMan;
        private Vector2 oldCursorPos;
        private KeyboardState oldKeyboardState;
        private MouseState oldMouseState;
        private float oldWheelPos;

        #endregion Private Fields

        #region Public Constructors

        public InputsMan(
            GameWindow gameWindow,
            IEventsMan eventsMan)
        {
            this.gameWindow = gameWindow;
            this.eventsMan = eventsMan;

            gameWindow.MouseMove += OnMouseMove;
            gameWindow.MouseWheel += OnMouseWheel;
            gameWindow.KeyDown += OnKeyDown;
            gameWindow.KeyUp += OnKeyUp;
            gameWindow.MouseDown += OnMouseDown;
            gameWindow.MouseUp += OnMouseUp;

            oldKeyboardState = gameWindow.KeyboardState.GetSnapshot();
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
        public Vector2 CursorPos { get; private set; }

        /// <summary>
        /// Gets wheel delta (difference between current and previous)
        /// </summary>
        public float WheelDelta { get; private set; }

        /// <summary>
        /// Gets cursor wheel value
        /// </summary>
        public float WheelPos { get; private set; }

        public bool IsMousePressed => gameWindow.MouseState.IsAnyButtonDown;

        public bool IsRightMousePressed => gameWindow.MouseState.IsButtonPressed(MouseButton.Right);

        #endregion Public Properties

        #region Public Methods

        public bool IsKeyPressed(int inputCode)
        {
            return gameWindow.KeyboardState.IsKeyDown((Keys)inputCode);
        }

        public void Update()
        {
            var newKeyboardState = gameWindow.KeyboardState.GetSnapshot();
            var newMouseState = gameWindow.MouseState.GetSnapshot();
            try
            {
                if (!newKeyboardState.Equals(oldKeyboardState))
                    OnKeyboardStateChanged(newKeyboardState);

                CursorDelta = CursorPos - oldCursorPos;
                oldCursorPos = CursorPos;

                WheelDelta = WheelPos - oldWheelPos;
                oldWheelPos = WheelPos;
            }
            finally
            {
                oldKeyboardState = newKeyboardState;
                oldMouseState = newMouseState;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void OnKeyboardStateChanged(KeyboardState keyboardState)
        {
            KeyboardStateChanged?.Invoke(this, new KeyboardStateEventArgs(oldKeyboardState, keyboardState));
        }

        #endregion Protected Methods

        #region Private Methods

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

        private void OnMouseMove(MouseMoveEventArgs e)
        {
            eventsMan.Raise(new MouseMoveEvent());
            //UpdateCursorPos(new Vector2(e.Position.X, e.Position.Y));
        }

        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            eventsMan.Raise(new MouseWheelEvent());
            //UpdateWheelPos(e.OffsetY);
        }

        //private void UpdateCursorPos(Vector2 newPos)
        //{
        //    var newPos4 = new Vector4(newPos) { W = 1 };
        //    newPos4 *= clientMan.ClientTransform;
        //    CursorPos = new Vector2(newPos4.X, newPos4.Y);
        //}

        private void UpdateWheelPos(float newWheelPos)
        {
            WheelPos = newWheelPos;
        }

        #endregion Private Methods
    }
}
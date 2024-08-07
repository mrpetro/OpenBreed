﻿using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Input.Generic
{
    internal class InputsMan : IInputsMan
    {
        #region Private Fields

        private readonly IViewClient clientMan;
        private readonly IEventsMan eventsMan;
        private Vector2 oldCursorPos;
        private KeyboardState oldKeyboardState;
        private MouseState oldMouseState;
        private float oldWheelPos;

        #endregion Private Fields

        #region Public Constructors

        public InputsMan(
            IViewClient clientMan,
            IEventsMan eventsMan)
        {
            this.clientMan = clientMan;
            this.eventsMan = eventsMan;

            clientMan.KeyDownEvent += (a) => OnKeyDown(a);
            clientMan.KeyUpEvent += (a) => OnKeyUp(a);
            //clientMan.KeyPressEvent += (s, a) => OnKeyPress(a);
            clientMan.MouseMoveEvent += (a) => OnMouseMove(a);
            clientMan.MouseDownEvent += (a) => OnMouseDown(a);
            clientMan.MouseUpEvent += (a) => OnMouseUp(a);
            clientMan.MouseWheelEvent += (a) => OnMouseWheel(a);

            oldKeyboardState = clientMan.KeyboardState.GetSnapshot();
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<KeyboardStateEventArgs> KeyboardStateChanged;

        public event EventHandler<MouseButtonEventArgs> MouseDown;

        public event EventHandler<MouseMoveEventArgs> MouseMove;

        public event EventHandler<MouseButtonEventArgs> MouseUp;

        //public event EventHandler<KeyPressEventArgs> KeyPress;
        public event EventHandler<MouseWheelEventArgs> MouseWheel;

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

        #endregion Public Properties

        #region Public Methods

        public bool IsPressed(int inputCode)
        {
            return clientMan.KeyboardState.IsKeyDown((Keys)inputCode);
        }

        public void Update()
        {
            var newKeyboardState = clientMan.KeyboardState.GetSnapshot();
            var newMouseState = clientMan.MouseState.GetSnapshot();
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
            eventsMan.Raise(this, new KeyDownEvent());
            //KeyDown?.Invoke(this, e);
        }

        private void OnKeyUp(KeyboardKeyEventArgs e)
        {
            eventsMan.Raise(this, new KeyUpEvent());
            //KeyUp?.Invoke(this, e);
        }

        private void OnMouseDown(MouseButtonEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        private void OnMouseMove(MouseMoveEventArgs e)
        {
            MouseMove?.Invoke(this, e);

            UpdateCursorPos(new Vector2(e.Position.X, e.Position.Y));
        }

        //private void OnKeyPress(KeyPressEventArgs e)
        //{
        //    KeyPress?.Invoke(this, e);
        //}
        private void OnMouseUp(MouseButtonEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }

        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            MouseWheel?.Invoke(this, e);

            UpdateWheelPos(e.OffsetY);
        }

        private void UpdateCursorPos(Vector2 newPos)
        {
            var newPos4 = new Vector4(newPos) { W = 1 };
            newPos4 *= clientMan.ClientTransform;
            CursorPos = new Vector2(newPos4.X, newPos4.Y);
        }

        private void UpdateWheelPos(float newWheelPos)
        {
            WheelPos = newWheelPos;
        }

        #endregion Private Methods
    }
}
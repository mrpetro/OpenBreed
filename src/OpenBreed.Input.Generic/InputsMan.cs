using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Input.Generic
{
    public class KeyboardStateEventArgs : EventArgs
    {
        #region Public Constructors

        public KeyboardStateEventArgs(KeyboardState oldState, KeyboardState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        #endregion Public Constructors

        #region Public Properties

        public KeyboardState NewState { get; }
        public KeyboardState OldState { get; }

        #endregion Public Properties

        #region Public Methods

        public bool IsKeyDown(Keys key)
        {
            return !OldState[key] && NewState[key];
        }

        public bool IsKeyUp(Keys key)
        {
            return OldState[key] && !NewState[key];
        }

        #endregion Public Methods
    }

    internal class InputsMan : IInputsMan
    {
        #region Private Fields

        private readonly IViewClient clientMan;
        private readonly IEventsMan eventsMan;
        private readonly Dictionary<Keys, KeyBinding> keyBindings = new Dictionary<Keys, KeyBinding>();
        private Dictionary<string, IInputHandler> controlHandlers = new Dictionary<string, IInputHandler>();
        private Vector2 oldCursorPos;
        private KeyboardState oldKeyboardState;
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

            KeyboardStateChanged += InputsMan_KeyboardStateChanged;

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

        public void AddPlayerKeyBinding(IPlayer player, string controlType, string controlAction, Keys key)
        {
            var controlHandler = GetHandler(controlType);

            keyBindings.Add(key, new KeyBinding(player, controlHandler, controlAction));
        }

        public IInputHandler GetHandler(string controlType)
        {
            Debug.Assert(controlType != null, "Control type is null");

            IInputHandler handler;

            controlHandlers.TryGetValue(controlType, out handler);

            return handler;
        }

        public void RegisterHandler(IInputHandler handler)
        {
            var controlType = handler.InputType;

            if (controlHandlers.ContainsKey(controlType))
                throw new InvalidOperationException($"Control type handler '{controlType}' already registered.");

            controlHandlers.Add(controlType, handler);
        }

        public void Update()
        {
            var newKeyboardState = clientMan.KeyboardState.GetSnapshot();

            try
            {
                CheckKeysPressed(newKeyboardState);

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

        private void CheckKeysPressed(KeyboardState state)
        {
            foreach (var keyBinding in keyBindings)
            {
                if (state[keyBinding.Key])
                    keyBinding.Value.OnKeyPressed();
            }
        }

        private void InputsMan_KeyboardStateChanged(object sender, KeyboardStateEventArgs e)
        {
            foreach (var keyBinding in keyBindings)
            {
                if (e.IsKeyDown(keyBinding.Key))
                    keyBinding.Value.OnKeyDown(1.0f);
                else if (e.IsKeyUp(keyBinding.Key))
                    keyBinding.Value.OnKeyUp(0.0f);
            }
        }

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
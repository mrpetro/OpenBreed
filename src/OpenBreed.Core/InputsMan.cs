using OpenTK;
using OpenTK.Input;
using System;

namespace OpenBreed.Core
{
    public class InputsMan
    {
        #region Private Fields

        private float oldWheelPos;
        private Vector2 oldCursorPos;

        #endregion Private Fields

        #region Public Constructors

        public InputsMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<MouseButtonEventArgs> MouseDown;

        public event EventHandler<MouseButtonEventArgs> MouseUp;

        public event EventHandler<KeyboardKeyEventArgs> KeyDown;

        public event EventHandler<KeyboardKeyEventArgs> KeyUp;

        public event EventHandler<KeyPressEventArgs> KeyPress;

        public event EventHandler<MouseMoveEventArgs> MouseMove;

        public event EventHandler<MouseWheelEventArgs> MouseWheel;

        #endregion Public Events

        #region Public Properties

        public ICore Core { get; }

        /// <summary>
        /// Gets cursor position in window coordinates
        /// </summary>
        public Vector2 CursorPos { get; private set; }

        /// <summary>
        /// Gets position delta (difference between current and previous)
        /// </summary>
        public Vector2 CursorDelta { get; private set; }

        /// <summary>
        /// Gets cursor wheel value
        /// </summary>
        public float WheelPos { get; private set; }

        /// <summary>
        /// Gets wheel delta (difference between current and previous)
        /// </summary>
        public float WheelDelta { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void OnKeyDown(KeyboardKeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
        }

        public void OnKeyUp(KeyboardKeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        public void OnKeyPress(KeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);
        }

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }

        public void OnMouseMove(MouseMoveEventArgs e)
        {
            MouseMove?.Invoke(this, e);

            UpdateCursorPos(new Vector2(e.Position.X, e.Position.Y));
        }

        public void OnMouseWheel(MouseWheelEventArgs e)
        {
            MouseWheel?.Invoke(this, e);

            UpdateWheelPos(e.ValuePrecise);
        }

        public void Update()
        {
            CursorDelta = CursorPos - oldCursorPos;
            oldCursorPos = CursorPos;

            WheelDelta = WheelPos - oldWheelPos;
            oldWheelPos = WheelPos;
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateWheelPos(float newWheelPos)
        {
            WheelPos = newWheelPos;
        }

        private void UpdateCursorPos(Vector2 newPos)
        {
            var newPos4 = new Vector4(newPos) { W = 1 };
            newPos4 *= Core.ClientTransform;
            CursorPos = new Vector2(newPos4.X, newPos4.Y);
        }

        #endregion Private Methods
    }
}
using OpenBreed.Core.Inputs;
using OpenTK;
using OpenTK.Input;
using System;

namespace OpenBreed.Core.Managers
{
    public interface IInputsMan
    {
        #region Public Events

        public event EventHandler<KeyboardKeyEventArgs> KeyDown;

        public event EventHandler<KeyboardKeyEventArgs> KeyUp;

        public event EventHandler<KeyPressEventArgs> KeyPress;

        public event EventHandler<MouseMoveEventArgs> MouseMove;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets cursor position in client coordinates
        /// </summary>
        Vector2 CursorPos { get; }

        /// <summary>
        /// Gets position delta (difference between current and previous)
        /// </summary>
        Vector2 CursorDelta { get; }

        /// <summary>
        /// Gets cursor wheel value
        /// </summary>
        float WheelPos { get; }

        /// <summary>
        /// Gets wheel delta (difference between current and previous)
        /// </summary>
        float WheelDelta { get; }

        #endregion Public Properties

        #region Public Methods

        void AddPlayerKeyBinding(Player player, string controlType, string controlAction, Key key);

        void OnKeyDown(KeyboardKeyEventArgs e);

        void OnKeyUp(KeyboardKeyEventArgs e);

        void OnKeyPress(KeyPressEventArgs e);

        void OnMouseDown(MouseButtonEventArgs e);

        void OnMouseUp(MouseButtonEventArgs e);

        void OnMouseMove(MouseMoveEventArgs e);

        void RegisterHandler(IControlHandler handler);

        IControlHandler GetHandler(string controlType);

        void OnMouseWheel(MouseWheelEventArgs e);

        void Update();

        #endregion Public Methods
    }
}
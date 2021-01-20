using OpenBreed.Core;
using OpenTK;
using OpenTK.Input;
using System;

namespace OpenBreed.Input.Interface
{
    public interface IInputsMan
    {
        #region Public Events

        event EventHandler<KeyboardKeyEventArgs> KeyDown;

        event EventHandler<KeyboardKeyEventArgs> KeyUp;

        event EventHandler<KeyPressEventArgs> KeyPress;

        event EventHandler<MouseMoveEventArgs> MouseMove;

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

        void AddPlayerKeyBinding(IPlayer player, string controlType, string controlAction, Key key);

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
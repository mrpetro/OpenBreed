using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Input.Interface
{
    public interface IInputsMan
    {
        #region Public Events

        event EventHandler<MouseMoveEventArgs> MouseMove;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets position delta (difference between current and previous)
        /// </summary>
        Vector2 CursorDelta { get; }

        /// <summary>
        /// Gets cursor position in client coordinates
        /// </summary>
        Vector2 CursorPos { get; }

        /// <summary>
        /// Gets wheel delta (difference between current and previous)
        /// </summary>
        float WheelDelta { get; }

        /// <summary>
        /// Gets cursor wheel value
        /// </summary>
        float WheelPos { get; }

        #endregion Public Properties

        #region Public Methods

        void Update();

        bool IsPressed(int inputCode);

        #endregion Public Methods
    }

    public class KeyDownEvent : EventArgs
    {
        #region Public Constructors

        public KeyDownEvent()
        {
        }

        #endregion Public Constructors
    }

    public class KeyUpEvent : EventArgs
    {
        #region Public Constructors

        public KeyUpEvent()
        {
        }

        #endregion Public Constructors
    }
}
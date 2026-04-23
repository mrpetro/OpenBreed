using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Input.Abstractions
{
    public interface IInputsMan
    {
        #region Public Events

        event EventHandler<KeyboardStateEventArgs> KeyboardStateChanged;

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

        bool IsMousePressed { get; }

        bool IsLeftMousePressed { get; }

        bool IsRightMousePressed { get; }

        bool IsMiddleMousePressed { get; }

        #endregion Public Properties

        #region Public Methods

        bool IsKeyPressed(int inputCode);

        #endregion Public Methods
    }

    public class KeyboardStateEventArgs : EventArgs
    {
        #region Public Constructors

        public KeyboardStateEventArgs(KeyboardState keyboardState, IReadOnlySet<Keys> keysPressed, IReadOnlySet<Keys> keysReleased)
        {
            KeyboardState = keyboardState;
            KeysPressed = keysPressed;
            KeysReleased = keysReleased;
        }

        #endregion Public Constructors

        #region Public Properties

        public KeyboardState KeyboardState { get; }
        public IReadOnlySet<Keys> KeysPressed { get; }
        public IReadOnlySet<Keys> KeysReleased { get; }

        #endregion Public Properties

        #region Public Methods

        public bool IsKeyDown(Keys key)
        {
            return KeysPressed.Contains(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return KeysReleased.Contains(key);
        }

        #endregion Public Methods
    }
}
﻿using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Input.Interface
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

        bool IsRightMousePressed { get; }

        #endregion Public Properties

        #region Public Methods

        void Update();

        bool IsKeyPressed(int inputCode);

        #endregion Public Methods
    }

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
}
using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Input.Interface
{
    public interface IInputsMan
    {
        #region Public Events

        event EventHandler<KeyboardKeyEventArgs> KeyDown;

        event EventHandler<KeyboardKeyEventArgs> KeyUp;

        //event EventHandler<KeyPressEventArgs> KeyPress;

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

        void AddPlayerKeyBinding(IPlayer player, string controlType, string controlAction, Keys key);

        void RegisterHandler(IInputHandler handler);

        IInputHandler GetHandler(string controlType);

        void Update();

        #endregion Public Methods
    }
}
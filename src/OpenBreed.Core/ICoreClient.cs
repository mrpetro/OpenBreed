using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;

namespace OpenBreed.Core
{
    public interface ICoreClient
    {
        #region Public Events

        event EventHandler<MouseButtonEventArgs> MouseDownEvent;

        event EventHandler<MouseButtonEventArgs> MouseUpEvent;

        event EventHandler<MouseMoveEventArgs> MouseMoveEvent;

        event EventHandler<MouseWheelEventArgs> MouseWheelEvent;

        event EventHandler<KeyboardKeyEventArgs> KeyDownEvent;

        event EventHandler<KeyboardKeyEventArgs> KeyUpEvent;

        event EventHandler<KeyPressEventArgs> KeyPressEvent;

        event EventHandler<float> UpdateFrameEvent;

        event EventHandler<float> RenderFrameEvent;

        event EventHandler LoadEvent;

        event EventHandler<Size> ResizeEvent;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Client display transformation matrix
        /// </summary>
        Matrix4 ClientTransform { get; }

        /// <summary>
        /// Client display aspect ratio
        /// </summary>
        float ClientRatio { get; }

        /// <summary>
        /// Client display rectangle
        /// </summary>
        Rectangle ClientRectangle { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Exits the application
        /// TODO: Show not be part of this inteface
        /// </summary>
        void Exit();

        /// <summary>
        /// Start running main aplication loop
        /// TODO: Show not be part of this inteface
        /// </summary>
        void Run();

        #endregion Public Methods
    }
}
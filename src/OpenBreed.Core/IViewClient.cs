using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Drawing;

namespace OpenBreed.Core
{
    public interface IViewClient
    {
        #region Public Events

        event Action<MouseButtonEventArgs> MouseDownEvent;

        event Action<MouseButtonEventArgs> MouseUpEvent;

        event Action<MouseMoveEventArgs> MouseMoveEvent;

        event Action<MouseWheelEventArgs> MouseWheelEvent;

        event Action<KeyboardKeyEventArgs> KeyDownEvent;

        event Action<KeyboardKeyEventArgs> KeyUpEvent;

        //event EventHandler<KeyPressEventArgs> KeyPressEvent;

        event Action<float> UpdateFrameEvent;

        event Action<float> RenderFrameEvent;

        event Action LoadEvent;

        event Action<Vector2i> ResizeEvent;

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
        Box2i ClientRectangle { get; }

        KeyboardState KeyboardState { get; }

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
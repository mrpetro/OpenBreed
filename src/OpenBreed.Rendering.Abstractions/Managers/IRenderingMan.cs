using OpenBreed.Rendering.Abstractions.Events;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Rendering.Abstractions.Managers
{
    public delegate void ViewRenderHandler(IRenderView view, Matrix4 transform, float dt);

    public delegate void ViewResizeHandler(IRenderView view, float width, float height);

    public delegate void ViewCursorWheelHandler(IRenderView view, int cursorId, Vector2i position, int wheelDelta);

    public delegate void ViewCursorDownHandler(IRenderView view, int cursorId, Vector2i position, CursorKey cursorKey);

    public delegate void ViewCursorUpHandler(IRenderView view, int cursorId, Vector2i position, CursorKey cursorKey);

    public delegate void ViewCursorEnterHandler(IRenderView view, int cursorId, Vector2i position);

    public delegate void ViewCursorLeaveHandler(IRenderView view, int cursorId, Vector2i position);

    public delegate void ViewCursorMoveHandler(IRenderView view, int cursorId, Vector2i position);

    public delegate void ViewTextInputHandler(IRenderView view, string text);

    public delegate void ViewKeyboardKeyHandler(IRenderView view, Keys key, KeyModifiers modifiers);

    public enum MatrixMode
    {
        ModelView,
        Projection,
        Texture,
        Color
    }

    /// <summary>
    /// Rendering manager interface
    /// </summary>
    public interface IRenderingMan
    {
        #region Public Events

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Get current rendering frames per second
        /// </summary>
        float Fps { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Updates the state
        /// </summary>
        /// <param name="dt">delta time</param>
        void Update(float dt);

        #endregion Public Methods
    }
}
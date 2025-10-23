using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions.Events
{
    /// <summary>
    /// Abstract interaction on specific render view event.
    /// </summary>
    public abstract class ViewEvent : EventArgs
    {
        #region Protected Constructors

        protected ViewEvent(IRenderView view)
        {
            View = view;
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// View which is interacted with.
        /// </summary>
        public IRenderView View { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Abstract cursor interaction on specific render view event.
    /// </summary>
    public abstract class ViewCursorEvent : ViewEvent
    {
        #region Protected Constructors

        protected ViewCursorEvent(IRenderView view, int cursorId, Vector2i position) : base(view)
        {
            CursorId = cursorId;
            Position = position;
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Cursor ID which is interacting with view.
        /// </summary>
        public int CursorId { get; }

        /// <summary>
        /// Cursor position (in view coordinates)
        /// </summary>
        public Vector2i Position { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Occurs when cursor has moved over specific render view.
    /// </summary>
    public class ViewCursorMoveEvent : ViewCursorEvent
    {
        #region Private Fields

        private readonly BitArray keysPressed;

        #endregion Private Fields

        #region Public Constructors

        public ViewCursorMoveEvent(IRenderView view, int cursorId, BitArray keysPressed, Vector2i position) : base(view, cursorId, position)
        {
            this.keysPressed = keysPressed;
        }

        public bool IsCursorKeyPressed(CursorKey key)
        {
            return keysPressed[(int)key];
        }

        #endregion Public Constructors
    }

    /// <summary>
    /// Occurs when cursor has left from specific render view.
    /// </summary>
    public class ViewCursorLeaveEvent : ViewCursorEvent
    {
        #region Public Constructors

        public ViewCursorLeaveEvent(IRenderView view, int cursorId, Vector2i position) : base(view, cursorId, position)
        {
        }

        #endregion Public Constructors
    }

    /// <summary>
    /// Occurs when cursor has entered over specific render view.
    /// </summary>
    public class ViewCursorEnterEvent : ViewCursorEvent
    {
        #region Public Constructors

        public ViewCursorEnterEvent(IRenderView view, int cursorId, Vector2i position) : base(view, cursorId, position)
        {
        }

        #endregion Public Constructors
    }

    /// <summary>
    /// Occurs when specific cursor key has been released on specific render view.
    /// </summary>
    public class ViewCursorUpEvent : ViewCursorEvent
    {
        #region Public Constructors

        public ViewCursorUpEvent(IRenderView view, int cursorId, Vector2i position, CursorKey key, KeyModifiers modifiers) : base(view, cursorId, position)
        {
            Key = key;
            Modifiers = modifiers;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Key code of cursor which was released.
        /// </summary>
        public CursorKey Key { get; }

        /// <summary>
        /// Flags indicating if ore or more special keys were pressed during cursor key up event.
        /// </summary>
        public KeyModifiers Modifiers { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Occurs when specific cursor key has been pressed on specific render view.
    /// </summary>
    public class ViewCursorDownEvent : ViewCursorEvent
    {
        #region Public Constructors

        public ViewCursorDownEvent(IRenderView view, int cursorId, Vector2i position, CursorKey key, KeyModifiers modifiers) : base(view, cursorId, position)
        {
            Key = key;
            Modifiers = modifiers;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Key code of cursor which was pressed.
        /// </summary>
        public CursorKey Key { get; }

        /// <summary>
        /// Flags indicating if ore or more special keys were pressed during cursor key down event.
        /// </summary>
        public KeyModifiers Modifiers { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Occurs when cursor wheel position has changed.
    /// </summary>
    public class ViewCursorWheelEvent : ViewCursorEvent
    {
        #region Public Constructors

        public ViewCursorWheelEvent(IRenderView view, int cursorId, Vector2i position, int wheelDelta) : base(view, cursorId, position)
        {
            WheelDelta = wheelDelta;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Value indicating how much wheel position has changed.
        /// </summary>
        public int WheelDelta { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Occurs when specific keyboard key has been pressed on specific render view.
    /// </summary>
    public class ViewKeyDownEvent : ViewEvent
    {
        #region Public Constructors

        public ViewKeyDownEvent(IRenderView view, Keys key, KeyModifiers modifiers) : base(view)
        {
            Key = key;
            Modifiers = modifiers;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Key code of keyboard which was pressed.
        /// </summary>
        public Keys Key { get; }

        /// <summary>
        /// Flags indicating if ore or more special keys were pressed during keyboard key down event.
        /// </summary>
        public KeyModifiers Modifiers { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Occurs when specific keyboard key has been released on specific render view.
    /// </summary>
    public class ViewKeyUpEvent : ViewEvent
    {
        #region Public Constructors

        public ViewKeyUpEvent(IRenderView view, Keys key, KeyModifiers modifiers) : base(view)
        {
            Key = key;
            Modifiers = modifiers;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Key code of keyboard which was released.
        /// </summary>
        public Keys Key { get; }

        /// <summary>
        /// Flags indicating if ore or more special keys were pressed during keyboard key up event.
        /// </summary>
        public KeyModifiers Modifiers { get; }

        #endregion Public Properties
    }
}
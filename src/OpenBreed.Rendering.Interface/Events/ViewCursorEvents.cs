using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Events
{
    /// <summary>
    /// Abstract cursor interaction on specific render view event.
    /// </summary>
    public abstract class ViewCursorEvent : EventArgs
    {
        #region Protected Constructors

        protected ViewCursorEvent(IRenderView view, int cursorId)
        {
            View = view;
            CursorId = cursorId;
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// View which cursor interacts with.
        /// </summary>
        public IRenderView View { get; }

        /// <summary>
        /// Cursor ID which is interacting with view.
        /// </summary>
        public int CursorId { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Occurs when cursor has moved over specific render view.
    /// </summary>
    public class ViewCursorMoveEvent : ViewCursorEvent
    {
        #region Public Constructors

        public ViewCursorMoveEvent(IRenderView view, int cursorId, Vector2i position) : base(view, cursorId)
        {
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Cursor position (in view coordinates)
        /// </summary>
        public Vector2i Position { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Occurs when cursor has left from specific render view.
    /// </summary>
    public class ViewCursorLeaveEvent : ViewCursorEvent
    {
        #region Public Constructors

        public ViewCursorLeaveEvent(IRenderView view, int cursorId) : base(view, cursorId)
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

        public ViewCursorEnterEvent(IRenderView view, int cursorId) : base(view, cursorId)
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

        public ViewCursorUpEvent(IRenderView view, int cursorId, int keyCode) : base(view, cursorId)
        {
            KeyCode = keyCode;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Key code of cursor which was released.
        /// </summary>
        public int KeyCode { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Occurs when specific cursor key has been pressed on specific render view.
    /// </summary>
    public class ViewCursorDownEvent : ViewCursorEvent
    {
        #region Public Constructors

        public ViewCursorDownEvent(IRenderView view, int cursorId, int keyCode) : base(view, cursorId)
        {
            KeyCode = keyCode;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Key code of cursor which was pressed.
        /// </summary>
        public int KeyCode { get; }

        #endregion Public Properties
    }
}
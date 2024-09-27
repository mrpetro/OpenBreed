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
    /// Cursor key names with their values.
    /// </summary>
    public enum CursorKeys
    {
        /// <summary>
        /// Left cursor key.
        /// </summary>
        Left = 0,
        /// <summary>
        /// Middle cursor key.
        /// </summary>
        Middle = 1,
        /// <summary>
        /// Right cursor key.
        /// </summary>
        Right = 2,
        /// <summary>
        /// First extended cursor key.
        /// </summary>
        XButton1 = 3,
        /// <summary>
        /// Second extended cursor key.
        /// </summary>
        XButton2 = 4
    }

    /// <summary>
    /// Abstract cursor interaction on specific render view event.
    /// </summary>
    public abstract class ViewCursorEvent : EventArgs
    {
        #region Protected Constructors

        protected ViewCursorEvent(IRenderView view, int cursorId, Vector2i position)
        {
            View = view;
            CursorId = cursorId;
            Position = position;
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
        #region Public Constructors

        public ViewCursorMoveEvent(IRenderView view, int cursorId, Vector2i position) : base(view, cursorId, position)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties
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

        public ViewCursorUpEvent(IRenderView view, int cursorId, Vector2i position, CursorKeys key) : base(view, cursorId, position)
        {
            Key = key;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Key code of cursor which was released.
        /// </summary>
        public CursorKeys Key { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Occurs when specific cursor key has been pressed on specific render view.
    /// </summary>
    public class ViewCursorDownEvent : ViewCursorEvent
    {
        #region Public Constructors

        public ViewCursorDownEvent(IRenderView view, int cursorId, Vector2i position, CursorKeys key) : base(view, cursorId, position)
        {
            Key = key;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Key code of cursor which was pressed.
        /// </summary>
        public CursorKeys Key { get; }

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
}
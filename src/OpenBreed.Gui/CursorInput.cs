using OpenBreed.Gui.Abstractions;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenBreed.Gui
{
    public class CursorInput : ICursorInputHandler
    {
        #region Private Fields

        private readonly Dictionary<int, InteractionCursor> cursors = new Dictionary<int, InteractionCursor>();

        private readonly InteractionCore interactionCore;

        #endregion Private Fields

        #region Internal Constructors

        internal CursorInput(InteractionCore interactionCore)
        {
            this.interactionCore = interactionCore;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Down(int cursorId, CursorKey key)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdateButton(key, pressed: true);

            interactionCore.OnCursorDown(cursor, key);
        }

        public void Enter(int cursorId)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdateEnabled(enabled: true);

            interactionCore.OnCursorEnter(cursor);
        }

        public void Leave(int cursorId)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdateEnabled(enabled: false);

            interactionCore.OnCursorLeave(cursor);
        }

        public void Move(int cursorId, Vector2 position)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdatePosition(position);

            interactionCore.OnCursorMove(cursor);
        }

        public void Up(int cursorId, CursorKey key)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdateButton(key, pressed: false);

            interactionCore.OnCursorUp(cursor, key);
        }

        public void Wheel(int cursorId, int wheelDelta)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdateWheel(wheelDelta);

            interactionCore.OnCursorWheel(cursor);
        }

        #endregion Public Methods

        #region Private Methods

        private InteractionCursor GetCursor(int cursorId)
        {
            if (!cursors.TryGetValue(cursorId, out InteractionCursor? cursor))
            {
                cursor = new InteractionCursor(cursorId);

                cursors.Add(cursorId, cursor);
            }

            return cursor;
        }

        #endregion Private Methods
    }
}
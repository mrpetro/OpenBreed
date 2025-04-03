using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Interface.Events;
using KeyModifiers = OpenBreed.Rendering.Interface.Events.KeyModifiers;
using Keys = OpenBreed.Rendering.Interface.Events.Keys;

namespace OpenBreed.Gui.Abstractions.Controllers
{
    public abstract class ElementInputHandler<TElement> : IElementInputHandler<TElement> where TElement : IElement
    {
        #region Public Methods

        public void OnCursorClick(IElement element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            OnCursorClick((TElement)element, cursor, cursorKey);
        }

        public void OnCursorDown(IElement element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            OnCursorDown((TElement)element, cursor, cursorKey);
        }

        public void OnCursorDrag(IElement element, IInteractionCursor cursor)
        {
            OnCursorDrag((TElement)element, cursor);
        }

        public void OnCursorEnter(IElement element, IInteractionCursor cursor)
        {
            OnCursorEnter((TElement)element, cursor);
        }

        public void OnCursorLeave(IElement element, IInteractionCursor cursor)
        {
            OnCursorLeave((TElement)element, cursor);
        }

        public void OnCursorMove(IElement element, IInteractionCursor cursor)
        {
            OnCursorMove((TElement)element, cursor);
        }

        public void OnCursorUp(IElement element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            OnCursorUp((TElement)element, cursor, cursorKey);
        }

        public void OnCursorWheel(IElement element, IInteractionCursor cursor)
        {
            OnCursorWheel((TElement)element, cursor);
        }

        public void OnKeyboardKeyDown(IElement element, Keys key, KeyModifiers modifiers)
        {
            OnKeyboardKeyDown((TElement)element, key, modifiers);
        }

        public void OnKeyboardKeyUp(IElement element, Keys key, KeyModifiers modifiers)
        {
            OnKeyboardKeyUp((TElement)element, key, modifiers);
        }

        public void OnKeyboardTextInput(IElement element, string text)
        {
            OnKeyboardTextInput((TElement)element, text);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void OnCursorClick(TElement element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            if (cursorKey == CursorKey.Left)
            {
                element.Click(cursor, cursorKey);
            }
        }

        protected virtual void OnCursorDrag(TElement element, IInteractionCursor cursor)
        {
            if (element.IsMovable)
            {
                element.MoveBy(cursor.PositionDelta);
            }
        }

        protected virtual void OnCursorEnter(TElement element, IInteractionCursor cursor)
        {
            element.Enter(cursor);
        }

        protected virtual void OnCursorLeave(TElement element, IInteractionCursor cursor)
        {
            element.Leave(cursor);
        }

        protected virtual void OnCursorMove(TElement element, IInteractionCursor cursor)
        {
        }

        protected virtual void OnCursorUp(TElement element, IInteractionCursor cursor, CursorKey cursorKey)
        {
        }

        protected virtual void OnCursorDown(TElement element, IInteractionCursor cursor, CursorKey cursorKey)
        {
        }

        protected virtual void OnCursorWheel(TElement element, IInteractionCursor cursor)
        {
        }

        protected virtual void OnKeyboardKeyDown(TElement element, Keys key, KeyModifiers modifiers)
        {
        }

        protected virtual void OnKeyboardKeyUp(TElement element, Keys key, KeyModifiers modifiers)
        {
        }

        protected virtual void OnKeyboardTextInput(TElement element, string text)
        {
        }

        #endregion Protected Methods
    }
}
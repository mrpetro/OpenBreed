using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Windowing.GraphicsLibraryFramework;
using KeyModifiers = OpenBreed.Rendering.Interface.Events.KeyModifiers;
using Keys = OpenBreed.Rendering.Interface.Events.Keys;

namespace OpenBreed.Gui.Abstractions.Controllers
{
    public interface IElementInputHandler
    {
        #region Public Methods

        void OnCursorMove(IElement element, IInteractionCursor cursor);

        void OnCursorWheel(IElement element, IInteractionCursor cursor);

        void OnCursorEnter(IElement element, IInteractionCursor cursor);

        void OnCursorLeave(IElement element, IInteractionCursor cursor);

        void OnCursorClick(IElement element, IInteractionCursor cursor, CursorKey cursorKey);

        void OnCursorDown(IElement element, IInteractionCursor cursor, CursorKey cursorKey);

        void OnCursorUp(IElement element, IInteractionCursor cursor, CursorKey cursorKey);

        void OnCursorDrag(IElement element, IInteractionCursor cursor);

        void OnKeyboardTextInput(IElement element, string text);

        void OnKeyboardKeyDown(IElement element, Keys key, KeyModifiers modifiers);

        void OnKeyboardKeyUp(IElement element, Keys key, KeyModifiers modifiers);

        #endregion Public Methods
    }

    public interface IElementInputHandler<TElement> : IElementInputHandler where TElement : IElement
    {
    }
}
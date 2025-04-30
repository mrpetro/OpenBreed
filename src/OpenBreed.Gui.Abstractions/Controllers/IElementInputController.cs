using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Abstractions.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using KeyModifiers = OpenBreed.Rendering.Abstractions.Events.KeyModifiers;
using Keys = OpenBreed.Rendering.Abstractions.Events.Keys;

namespace OpenBreed.Gui.Abstractions.Controllers
{
    public interface IElementInputController
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

    public interface IElementInputController<TElement> : IElementInputController where TElement : IElement
    {
    }
}
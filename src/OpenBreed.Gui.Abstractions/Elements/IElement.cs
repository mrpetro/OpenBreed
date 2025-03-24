using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Presentations;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public enum ElementDockMode
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
        Fill
    }

    public enum ElementResizeAnchor
    {
        Center,
        Left,
        LeftTop,
        Top,
        TopRight,
        Right,
        RightBottom,
        Bottom,
        BottomLeft
    }

    /// <summary>
    /// Element that user can interact with.
    /// </summary>
    public interface IElement
    {
        #region Public Properties

        Box2 Padding { get; }
        Box2 Margin { get; }

        IList<IElementOption> Options { get; }

        IElementPosition Position { get; }

        Vector2 MinimumSize { get; }
        Vector2 MaximumSize { get; }
        IElementSize Size { get; }

        Box2 LocalBox { get; }
        Box2 ActualBox { get; }

        bool IsHovered { get; }
        bool IsMovable { get; }

        IElement? Parent { get; }

        string Tag { get; }

        #endregion Public Properties

        #region Public Methods

        Box2 ToWorld(Box2 box);

        Vector2 ToWorld(Vector2 position);

        void MoveBy(Vector2 offset);

        void Resize(Vector2 newSize);

        void ResizeBy(Vector2 offset, ElementResizeAnchor anchor);

        void OnCursorMove(IInteractionCursor cursor);

        void OnCursorWheel(IInteractionCursor cursor);

        void OnCursorEnter(IInteractionCursor cursor);

        void OnCursorLeave(IInteractionCursor cursor);

        void OnCursorClick(IInteractionCursor cursor, CursorKey cursorKey);

        void OnCursorDown(IInteractionCursor cursor, CursorKey cursorKey);

        void OnCursorUp(IInteractionCursor cursor, CursorKey cursorKey);

        void OnKeyboardTextInput(string text);

        void OnKeyboardKeyDown(Keys key, KeyModifiers modifiers);

        void OnKeyboardKeyUp(Keys key, KeyModifiers modifiers);

        bool HitTest(Vector2 point, out IElement? interactiveElement);

        IElement? GetAncestor(string tag);

        #endregion Public Methods
    }
}
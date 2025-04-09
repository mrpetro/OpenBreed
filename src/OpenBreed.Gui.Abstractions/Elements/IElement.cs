using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Controllers;
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

        IElementInputController InputController { get; }

        Box2 LocalBox { get; }
        Box2 ActualBox { get; }

        bool IsHovered { get; }
        bool IsMovable { get; }

        IElement? Parent { get; }

        string Tag { get; }

        bool IsFocused { get; }

        #endregion Public Properties

        #region Public Methods

        Box2 ToWorld(Box2 box);

        Vector2 ToWorld(Vector2 position);

        void MoveBy(Vector2 offset);

        void Resize(Vector2 newSize);

        /// <summary>
        /// Focus on this element.
        /// </summary>
        void Focus();

        /// <summary>
        /// Unfocus from this element.
        /// </summary>
        void Unfocus();

        /// <summary>
        /// Click this element with given cursor.
        /// </summary>
        /// <param name="cursor">Clicking cursor.</param>
        /// <param name="cursorKey">Clicking cursor key.</param>
        void Click(IInteractionCursor cursor, CursorKey cursorKey);

        /// <summary>
        /// Enter this element with given cursor.
        /// </summary>
        /// <param name="cursor">Entering cursor.</param>
        void Enter(IInteractionCursor cursor);

        /// <summary>
        /// Leave this element with given cursor.
        /// </summary>
        /// <param name="cursor">Leaving cursor.</param>
        void Leave(IInteractionCursor cursor);

        void ResizeBy(Vector2 offset, ElementResizeAnchor anchor);

        bool HitTest(Vector2 point, out IElement? interactiveElement);

        IElement? GetAncestor(string tag);

        #endregion Public Methods
    }
}
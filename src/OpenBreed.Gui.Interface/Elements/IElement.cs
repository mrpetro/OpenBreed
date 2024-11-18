using OpenBreed.Gui.Interface.Bodies;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Elements
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

    /// <summary>
    /// Element that user can interact with.
    /// </summary>
    public interface IElement
    {
        #region Public Properties

        Box2 Padding { get; }
        Box2 Margin { get; }

        IElementPosition Position { get; }

        ElementDockMode DockMode { get; }

        IElementBody Body { get; }

        bool IsHovered { get; }

        IElement? Parent { get; }

        IReadOnlyList<IElement> Childs { get; }

        string Tag { get; }

        #endregion Public Properties

        #region Public Methods

        void OnMove(int cursorId, Vector2 position);

        void OnWheel(int cursorId, int delta);

        void OnEnter(int cursorId);

        void OnLeave(int cursorId);

        void OnClick(int cursorId, CursorKey cursorKey);

        void OnDown(int cursorId, Vector2 position, CursorKey cursorKey);

        void OnUp(int cursorId, Vector2 position, CursorKey cursorKey);

        bool HitTest(Vector2 point, out IElement? interactiveElement);

        #endregion Public Methods
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface
{
    /// <summary>
    /// Element that user can interact with.
    /// </summary>
    public interface IInteractiveElement
    {
        #region Public Properties

        float CenterX { get; }
        float CenterY { get; }
        float Width { get; }
        float Height { get; }

        IInteractiveElement? Parent { get; }

        IReadOnlyList<IInteractiveElement> Childs { get; }

        string Tag { get; }

        #endregion Public Properties

        #region Public Methods

        void OnMove(int cursorId, float x, float y);

        void OnWheel(int cursorId, int delta);

        void OnEnter(int cursorId);

        void OnLeave(int cursorId);

        void OnClick(int cursorId, CursorKey cursorKey);

        void OnDown(int cursorId, CursorKey cursorKey);

        void OnUp(int cursorId, CursorKey cursorKey);

        bool HitTest(float x, float y, out IInteractiveElement? interactiveElement);

        #endregion Public Methods
    }
}
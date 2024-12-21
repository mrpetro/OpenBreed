using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions
{
    public interface ICursorInputHandler
    {
        void Down(int cursorId, CursorKey key);
        void Enter(int cursorId);
        void Leave(int cursorId);
        void Move(int cursorId, Vector2 position);
        void Up(int cursorId, CursorKey key);
        void Wheel(int cursorId, int wheelDelta);
    }

    public interface IInteractionCore
    {
        #region Public Methods

        bool HitTest(Vector2 position, out IElement? interactiveElement);

        IElement? Root { get; set; }

        IElementBuilder CreateDockPanel(Action<IDockPanelBuilder> setter);

        ICursorInputHandler CreateCursorInput();

        #endregion Public Methods
    }
}
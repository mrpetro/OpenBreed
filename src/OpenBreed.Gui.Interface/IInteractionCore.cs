using OpenBreed.Gui.Interface.Builders;
using OpenBreed.Gui.Interface.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface
{
    public interface IInteractionCore
    {
        #region Public Methods

        bool HitTest(float x, float y, out IElement? interactiveElement);

        IElement? Root { get; set; }

        IElementBuilder CreateDockPanel(Action<IDockPanelBuilder> setter);

        void Enter(int cursorId, float x, float y);

        void Move(int cursorId, float x, float y);

        void Wheel(int cursorId, float x, float y, int delta);

        void Leave(int cursorId);

        void Down(int cursorId, float x, float y, CursorKey cursorKey);

        void Up(int cursorId, float x, float y, CursorKey cursorKey);

        #endregion Public Methods
    }
}
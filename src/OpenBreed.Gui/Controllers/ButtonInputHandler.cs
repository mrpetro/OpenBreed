using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface.Events;
using System.Reflection;

namespace OpenBreed.Gui.Controllers
{
    public class ButtonInputHandler : ElementInputHandler<IButton>
    {
        public ButtonInputHandler()
        {
        }

        protected override void OnCursorDown(IButton element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            element.Press();
        }

        protected override void OnCursorUp(IButton element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            element.Release();
        }
    }
}
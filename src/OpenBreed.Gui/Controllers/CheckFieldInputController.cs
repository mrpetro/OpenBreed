using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface.Events;
using System.Reflection;

namespace OpenBreed.Gui.Controllers
{
    public class CheckFieldInputController : ElementInputController<ICheckField>
    {
        public CheckFieldInputController()
        {
        }

        protected override void OnCursorClick(ICheckField element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            element.Click(cursor, cursorKey);
        }
    }
}
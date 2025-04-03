using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface.Events;
using System.Reflection;

namespace OpenBreed.Gui.Controllers
{
    public class CheckboxInputHandler : ElementInputHandler<ICheckbox>
    {
        public CheckboxInputHandler()
        {
        }

        protected override void OnCursorClick(ICheckbox element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            element.Click(cursor, cursorKey);
        }
    }
}
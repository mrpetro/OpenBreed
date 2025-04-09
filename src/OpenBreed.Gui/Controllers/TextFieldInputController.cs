using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface.Events;
using System.Reflection;

namespace OpenBreed.Gui.Controllers
{
    public class TextFieldInputController : ElementInputController<ITextField>
    {
        public TextFieldInputController()
        {
        }

        protected override void OnCursorClick(ITextField element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            element.Focus();
        }

        protected override void OnCursorDown(ITextField element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            if (cursor.IsPressed(CursorKey.Left))
            {
                var cursorPos = cursor.GetPositionRelativeTo(element);
                element.Pointer.SetIndexPosition(cursorPos);
            }
        }

        protected override void OnCursorDrag(ITextField element, IInteractionCursor cursor)
        {
            //element.ScrollPosition += cursor.PositionDelta;
        }

        protected override void OnCursorEnter(ITextField element, IInteractionCursor cursor)
        {
        }

        protected override void OnCursorLeave(ITextField element, IInteractionCursor cursor)
        {
        }

        protected override void OnCursorMove(ITextField element, IInteractionCursor cursor)
        {
            if (cursor.IsPressed(CursorKey.Left))
            {
                var cursorPos = cursor.GetPositionRelativeTo(element);
                element.Pointer.SetIndexPosition(cursorPos);
            }
        }

        protected override void OnCursorUp(ITextField element, IInteractionCursor cursor, CursorKey cursorKey)
        {
        }

        protected override void OnCursorWheel(ITextField element, IInteractionCursor cursor)
        {
        }

        protected override void OnKeyboardKeyDown(ITextField element, Keys key, KeyModifiers modifiers)
        {
            switch (key)
            {
                case Keys.Right:
                    element.Pointer.MoveForward();
                    break;

                case Keys.Left:
                    element.Pointer.MoveBack();
                    break;

                case Keys.Down:
                    element.Pointer.MoveLineForward();
                    break;

                case Keys.Up:
                    element.Pointer.MoveLineBack();
                    break;

                case Keys.Home:
                    element.Pointer.MoveToLineBegin();
                    break;

                case Keys.End:
                    element.Pointer.MoveToLineEnd();
                    break;

                case Keys.Enter:
                    element.Input(Environment.NewLine);
                    break;

                case Keys.Backspace:
                    element.Remove();
                    break;

                //case Keys.Delete:
                //    OnDeleteKey();
                //    break;

                default:
                    break;
            }
        }

        protected override void OnKeyboardKeyUp(ITextField element, Keys key, KeyModifiers modifiers)
        {
        }

        protected override void OnKeyboardTextInput(ITextField element, string text)
        {
            element.Input(text);
        }
    }
}
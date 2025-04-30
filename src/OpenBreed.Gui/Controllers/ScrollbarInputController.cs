using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenBreed.Gui.Extensions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenTK.Mathematics;
using System.Reflection;

namespace OpenBreed.Gui.Controllers
{
    public class ScrollbarInputController : ElementInputController<IScrollbar>
    {
        public ScrollbarInputController()
        {
        }

        protected override void OnCursorDrag(IScrollbar element, IInteractionCursor cursor)
        {
            if (!element.IsHandleGrabbed)
            {
                return;
            }

            var cursorOnHandlePosition = cursor.GetPositionRelativeTo(element) - element.HandleBox.Center;

            element.MoveHandle(cursorOnHandlePosition);
        }

        protected override void OnCursorDown(IScrollbar element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            if (cursorKey != CursorKey.Left)
            {
                return;
            }

            if (!element.IsHandleHovered)
            {
                return;
            }

            var positionOnHandle = cursor.GetPositionRelativeTo(element) - element.HandleBox.Center;

            element.GrabHandle(positionOnHandle);
        }

        protected override void OnCursorUp(IScrollbar element, IInteractionCursor cursor, CursorKey cursorKey)
        {
            if (cursorKey != CursorKey.Left)
            {
                return;
            }

            element.ReleaseHandle();
        }

        protected override void OnCursorMove(IScrollbar element, IInteractionCursor cursor)
        {
            try
            {
                var handleBox = element.HandleBox;

                var cursorPos = cursor.GetPositionRelativeTo(element);
                if (handleBox.ContainsInclusive(cursorPos))
                {
                    if (!element.IsHandleHovered)
                    {
                        element.EnterHandle();
                    }

                    return;
                }

                if (element.IsHandleHovered)
                {
                    element.LeaveHandle();
                }
            }
            finally
            {
                base.OnCursorMove(element, cursor);
            }
        }

        protected override void OnCursorLeave(IScrollbar element, IInteractionCursor cursor)
        {
            if (element.IsHandleHovered)
            {
                element.LeaveHandle();
            }

            base.OnCursorLeave(element, cursor);
        }

    }
}
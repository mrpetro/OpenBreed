using OpenBreed.Rendering.Abstractions.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions
{
    public delegate void ViewRenderHandler(IRenderView view, float dt);

    public delegate void ViewResizeHandler(IRenderView view, float width, float height);

    public delegate void ViewCursorWheelHandler(IRenderView view, int cursorId, Vector2i position, int wheelDelta);

    public delegate void ViewCursorDownHandler(IRenderView view, int cursorId, Vector2i position, CursorKey cursorKey);

    public delegate void ViewCursorUpHandler(IRenderView view, int cursorId, Vector2i position, CursorKey cursorKey);

    public delegate void ViewCursorEnterHandler(IRenderView view, int cursorId, Vector2i position);

    public delegate void ViewCursorLeaveHandler(IRenderView view, int cursorId, Vector2i position);

    public delegate void ViewCursorMoveHandler(IRenderView view, int cursorId, Vector2i position);

    public delegate void ViewTextInputHandler(IRenderView view, string text);

    public delegate void ViewKeyboardKeyHandler(IRenderView view, Keys key, KeyModifiers modifiers);
}
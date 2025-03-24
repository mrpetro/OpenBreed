using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Abstractions
{
    public interface IInteractionCursor
    {
        int Id { get; }

        IRenderView View { get; }
        Vector2 Position { get; }
        Vector2 PositionDelta { get; }
        int WheelDelta { get; }

        Vector2 GetPositionRelativeTo(IElement element);

        bool IsPressed(CursorKey key);
    };
}
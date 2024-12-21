using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Abstractions
{
    public interface IInteractionCursor
    {
        int Id { get; }

        Vector2 Position { get; }
        Vector2 PositionDelta { get; }
        int WheelDelta { get; }

        bool IsPressed(CursorKey key);
    };
}
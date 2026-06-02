using OpenTK.Mathematics;

namespace OpenBreed.Pathfinding.Abstractions.Services
{
    public interface IPathfindTerain
    {
        #region Public Methods

        int GetId(Vector2i position);

        Vector2i GetPosition(int id);

        int GetWeight(int id);

        bool TryGetDownFromId(int id, out int downId);

        bool TryGetLeftFromId(int id, out int leftId);

        bool TryGetRightFromId(int id, out int rightId);

        bool TryGetUpFromId(int id, out int upId);

        #endregion Public Methods
    }
}
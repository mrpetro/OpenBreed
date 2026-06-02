using OpenBreed.Pathfinding.Abstractions.Services;

namespace OpenBreed.Pathfinding.Services
{
    internal interface IInternalPathfindJob : IPathfindJob
    {
        void Reset(PathfindRequest request);
        void Run();
        bool Step();
    }
}
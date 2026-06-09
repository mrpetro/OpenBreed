using OpenBreed.Core.Abstractions;
using System.Collections.Generic;

namespace OpenBreed.Pathfinding.Abstractions.Services
{
    public interface IPathfindFront
    {
        #region Public Properties

        int Id { get; }

        #endregion Public Properties

        #region Public Methods

        bool Step();

        #endregion Public Methods
    }

    public interface IPathfindJob
    {
        #region Public Properties

        int StartId { get; }
        int GoalId { get; }
        PathfindStatus Status { get; }
        ITopology Topology { get; }
        int Id { get; }
        object Tag { get; }
        IEnumerable<IPathfindFront> Fronts { get; }
        IReadOnlyDictionary<int, int> CameFrom { get; }

        #endregion Public Properties

        #region Public Methods

        IEnumerable<int> GetShortestPath();

        #endregion Public Methods
    }
}
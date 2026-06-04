using OpenBreed.Core.Abstractions;
using OpenTK.Mathematics;
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

        Vector2i Start { get; }
        Vector2i Goal { get; }
        PathfindStatus Status { get; }
        ITopology Topology { get; }
        int Id { get; }
        object Tag { get; }
        IEnumerable<IPathfindFront> Fronts { get; }

        #endregion Public Properties

        #region Public Methods

        IEnumerable<(Vector2i, Vector2i)> GetWaypoints();

        IEnumerable<Vector2i> GetShortestPath();

        #endregion Public Methods
    }
}
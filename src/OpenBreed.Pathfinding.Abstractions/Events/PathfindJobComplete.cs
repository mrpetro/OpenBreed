using OpenBreed.Pathfinding.Abstractions.Services;
using System;

namespace OpenBreed.Pathfinding.Abstractions.Events
{
    public class PathfindJobComplete : EventArgs
    {
        #region Public Constructors

        public PathfindJobComplete(IPathfindJob job)
        {
            Job = job;
        }

        #endregion Public Constructors

        #region Public Properties

        public IPathfindJob Job { get; }

        #endregion Public Properties
    }
}
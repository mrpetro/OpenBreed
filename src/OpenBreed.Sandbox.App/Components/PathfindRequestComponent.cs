using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.App.Components
{
    public class PathfindRequestComponent : IEntityComponent
    {
        #region Public Constructors

        public PathfindRequestComponent(int requestId, Vector2 goalPosition)
        {
            RequestId = requestId;
            GoalPosition = goalPosition;
        }

        #endregion Public Constructors

        #region Public Properties

        public int RequestId { get; }
        public Vector2 GoalPosition { get; }

        #endregion Public Properties
    }
}
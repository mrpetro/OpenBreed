using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.App.Components
{
    public class PathFollowRequestComponent : IEntityComponent
    {
        #region Public Constructors

        public PathFollowRequestComponent(IReadOnlyList<Vector2i> waypoints)
        {
            Waypoints = waypoints;

        }

        #endregion Public Constructors

        #region Public Properties

        public IReadOnlyList<Vector2i> Waypoints { get; }
        public int CurrentWaypointIndex { get; set; }

        public Vector2i GetCurrentWaypoint()
        {
            return Waypoints[CurrentWaypointIndex];
        }

        public bool IsEnd() => CurrentWaypointIndex >= Waypoints.Count - 1;

        #endregion Public Properties
    }
}
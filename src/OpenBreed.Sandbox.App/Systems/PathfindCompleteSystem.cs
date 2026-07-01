using OpenBreed.Pathfinding.Abstractions.Events;
using OpenBreed.Pathfinding.Abstractions.Services;
using OpenBreed.Sandbox.App.Components;
using OpenBreed.Wecs.Abstractions.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OpenBreed.Core.Abstractions.Extensions;

namespace OpenBreed.Sandbox.App.Systems
{
    public class PathfindCompleteSystem : IEventSystem<PathfindJobComplete>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IPathfindingService pathfindingService;

        #endregion Private Fields

        #region Public Constructors

        public PathfindCompleteSystem(IEntityMan entityMan, IPathfindingService pathfindingService)
        {
            this.entityMan = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            this.pathfindingService = pathfindingService ?? throw new ArgumentNullException(nameof(pathfindingService));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(PathfindJobComplete e, IWorld world)
        {
            var job = e.Job;

            if (!(job.Tag is IEntity entity))
            {
                return;
            }

            if (job.Status == PathfindStatus.Found)
            {
                if (job.Topology is not GridTopology gridTopology)
                {
                    throw new InvalidOperationException($"Expected {typeof(GridTopology)}");
                }

                var pathfindRequest = entity.Get<PathfindRequestComponent>();

                var waypoints = job.GetShortestPath().Skip(1).SkipLast(1)
                    .Select((id) => gridTopology.DataGrid.GetIndex(id)).Select(item => item.ToPosition(cellSize: 16, CellAnchor.Center)).ToList();

                waypoints.Add(pathfindRequest.GoalPosition);

                if (waypoints.Count > 0)
                {
                    var follower = entity.Get<WaypointFollowerComponent>();

                    follower.Waypoints = waypoints;
                    follower.CurrentWaypointIndex = 0;
                    follower.MoveSpeed = 128;
                    follower.ArrivalDistance = 8f;
                }
            }

            entity.Remove<PathfindRequestComponent>();
        }

        #endregion Public Methods
    }
}
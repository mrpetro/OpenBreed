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
     
                var waypoints = job.GetShortestPath().Select((id) => gridTopology.DataGrid.GetPosition(id)).ToArray();

                if (waypoints.Length > 0)
                {
                    entity.Add(new PathFollowRequestComponent(waypoints));
                }
            }

            entity.Remove<PathfindRequestComponent>();
        }

        #endregion Public Methods
    }
}
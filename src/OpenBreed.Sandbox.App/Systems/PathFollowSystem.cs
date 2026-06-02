using OpenBreed.Sandbox.App.Components;
using OpenBreed.Wecs.Core.Systems;
using System;

namespace OpenBreed.Sandbox.App.Systems
{
    [RequireEntityWith(typeof(PathFollowRequestComponent))]
    public class PathFollowSystem : UpdatableMatchingSystemBase
    {
        #region Public Constructors

        public PathFollowSystem(IWorldMan worldMan) : base(worldMan)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, Wecs.Abstractions.Primitives.IUpdateContext context)
        {
            var pathFollowRequestCmp = entity.Get<PathFollowRequestComponent>();
            var mapPosition = entity.Get<MapPositionComponent>();

            var currentWaypoint = pathFollowRequestCmp.GetCurrentWaypoint();

            mapPosition.Value = currentWaypoint;

            if (pathFollowRequestCmp.IsEnd())
            {
                entity.Remove<PathFollowRequestComponent>();
                return;
            }

            pathFollowRequestCmp.CurrentWaypointIndex++;
        }

        #endregion Protected Methods
    }
}
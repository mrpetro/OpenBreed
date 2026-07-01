using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Systems;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.App.Systems
{
    public class WaypointFollowerComponent : IEntityComponent
    {
        #region Public Fields

        public List<Vector2> Waypoints;
        public int CurrentWaypointIndex;
        public float MoveSpeed;
        public float ArrivalDistance;

        #endregion Public Fields
    }

    [RequireEntityWith(
        typeof(WaypointFollowerComponent),
        typeof(PositionComponent))]
    public class WaypointMovementSystem : UpdatableMatchingSystemBase
    {
        #region Public Constructors

        public WaypointMovementSystem(IWorldMan worldMan) : base(worldMan)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, Wecs.Abstractions.Primitives.IUpdateContext context)
        {
            var position = entity.Get<PositionComponent>();
            var follower = entity.Get<WaypointFollowerComponent>();

            if (follower.Waypoints == null ||
                follower.CurrentWaypointIndex >= follower.Waypoints.Count)
            {
                return;
            }

            var target = follower.Waypoints[follower.CurrentWaypointIndex];

            var toTarget = target - position.Value;
            var distance = toTarget.Length;
            var step = follower.MoveSpeed * context.Dt;

            // Arrived at current waypoint
            if (distance <= follower.ArrivalDistance)
            {
                if (distance < step)
                {
                    if (!SwitchWaypoint(follower, ref target))
                    {
                        return;
                    }
                }

                toTarget = target - position.Value;
                distance = toTarget.Length;
            }

            if (distance > 0f)
            {
                var direction = Vector2.Normalize(toTarget);
                position.Value += direction * step;
            }
        }

        private static bool SwitchWaypoint(WaypointFollowerComponent follower, ref Vector2 target)
        {
            follower.CurrentWaypointIndex++;

            // Finished path
            if (follower.CurrentWaypointIndex >= follower.Waypoints.Count)
            {
                return false;
            }

            target = follower.Waypoints[follower.CurrentWaypointIndex];
            return true;
        }

        #endregion Protected Methods
    }
}
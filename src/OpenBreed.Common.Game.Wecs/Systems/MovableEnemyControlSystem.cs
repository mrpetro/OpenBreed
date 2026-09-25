using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWithClass("MovableEnemy")]
    public class MovableEnemyControlSystem : IUpdatableSystem
    {
        public void Update(IEnumerable<IEntity> entities, IUpdateContext context)
        {
            foreach (var entity in entities)
            {
                var angularVelocity = entity.Get<AngularVelocityComponent>();

                var dx = (float)Random.Shared.NextDouble() - 0.5f;
                var dy = (float)Random.Shared.NextDouble() - 0.5f;

                var direction = new Vector2(dx, dy);

                if (direction != Vector2.Zero)
                {
                    var movement = entity.Get<MotionComponent>();
                    entity.Get<ThrustComponent>().Value = direction * movement.Acceleration;
                    angularVelocity.Value = direction;
                }
                else
                {
                    var thrust = entity.Get<ThrustComponent>();
                    thrust.Value = Vector2.Zero;

                    var angularPosition = entity.Get<AngularPositionComponent>();
                    angularVelocity.Value = angularPosition.Value;
                }
            }
        }
    }
}

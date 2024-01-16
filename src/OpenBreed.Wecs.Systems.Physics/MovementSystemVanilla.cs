using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems.Physics
{
    /// <summary>
    /// System which tries to replicate ABTA actor movement behavior
    /// </summary>
    [RequireEntityWith(
        typeof(ThrustComponent),
        typeof(PositionComponent),
        typeof(VelocityComponent),
        typeof(BodyComponent))]
    public class MovementSystemVanilla : UpdatableSystemBase<MovementSystemVanilla>
    {
        #region Private Fields

        private const float FLOOR_FRICTION = 0.0f;

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Internal Constructors

        internal MovementSystemVanilla(
            IEntityMan entityMan)
        {
            this.entityMan = entityMan;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var position = entity.Get<PositionComponent>();
            var thrust = entity.Get<ThrustComponent>();
            var velocity = entity.Get<VelocityComponent>();
            var dynamicBody = entity.Get<BodyComponent>();

            //Velocity equation
            var newVel = thrust.Value;

            //Apply friction force
            newVel += -newVel * FLOOR_FRICTION * dynamicBody.CofFactor;

            //Verlet integration
            var newPos = position.Value + (velocity.Value + newVel) * 0.5f * context.Dt;

            velocity.Value = newVel;
            position.Value = newPos;
            //if (position.Value == newPos)
            //    return;

            //position.Value = newPos;
            //entity.RaiseEvent(new PositionChangedEventArgs(position.Value));
        }

        #endregion Protected Methods
    }
}
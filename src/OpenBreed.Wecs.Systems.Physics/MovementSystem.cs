using OpenBreed.Core.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Events;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(
        typeof(ThrustComponent),
        typeof(PositionComponent),
        typeof(VelocityComponent),
        typeof(BodyComponent))]
    public class MovementSystem : UpdatableMatchingSystemBase<MovementSystem>
    {
        #region Private Fields

        private const float FLOOR_FRICTION = 0.2f;

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Internal Constructors

        internal MovementSystem(
            IEntityMan entityMan,
            IEventsMan eventsMan)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var position = entity.Get<PositionComponent>();
            var thrust = entity.Get<ThrustComponent>();
            var velocity = entity.Get<VelocityComponent>();
            var dynamicBody = entity.Get<BodyComponent>();

            //Velocity equation
            var newVel = velocity.Value + thrust.Value * context.Dt;

            //Apply friction force
            newVel += -newVel * FLOOR_FRICTION * dynamicBody.CofFactor;

            //Verlet integration
            var newPos = position.Value + (velocity.Value + newVel) * 0.5f * context.Dt;

            velocity.Value = newVel;

            if (position.Value == newPos)
                return;

            position.Value = newPos;
            eventsMan.Raise(new PositionChangedEvent(entity.Id, position.Value));
        }

        #endregion Protected Methods
    }
}
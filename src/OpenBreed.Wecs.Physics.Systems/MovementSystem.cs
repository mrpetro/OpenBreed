using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Components;
using OpenBreed.Wecs.Core.Systems;
using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Physics.Systems.Events;

namespace OpenBreed.Wecs.Physics.Systems
{
    [RequireEntityWith(
        typeof(ThrustComponent),
        typeof(PositionComponent),
        typeof(VelocityComponent),
        typeof(BodyComponent))]
    [SystemCategory(CommonCategories.Physics)]
    public class MovementSystem : UpdatableMatchingSystemBase
    {
        #region Private Fields

        private const float FLOOR_FRICTION = 0.2f;

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public MovementSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan) : base(worldMan)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

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
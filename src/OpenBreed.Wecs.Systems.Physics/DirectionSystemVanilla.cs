using OpenBreed.Core.Abstractions.Extensions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Wecs.Systems.Physics
{
    /// <summary>
    /// System which tries to replicate ABTA actor direction behavior
    /// </summary>
    [RequireEntityWith(
        typeof(AngularPositionComponent),
        typeof(AngularVelocityComponent),
        typeof(AngularThrustComponent))]

    public class DirectionSystemVanilla : UpdatableMatchingSystemBase
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Internal Constructors

        public DirectionSystemVanilla(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan) : base(worldMan)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var angularPos = entity.Get<AngularPositionComponent>();
            var angularVel = entity.Get<AngularVelocityComponent>();

            var aPos = angularPos.Value;
            var dPos = angularVel.Value;

            var newPos = aPos.RotateTowards(dPos, (float)Math.PI * 0.125f, 1.0f);

            if (newPos == aPos)
            {
                eventsMan.Raise(new DirectionSetEvent(entity.Id, angularPos.Value));
                return;
            }

            angularPos.Value = newPos;
            eventsMan.Raise(new DirectionChangedEvent(entity.Id, angularPos.Value));
        }

        #endregion Protected Methods
    }
}
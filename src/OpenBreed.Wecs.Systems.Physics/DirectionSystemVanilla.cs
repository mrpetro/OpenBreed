using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Events;
using System;

namespace OpenBreed.Wecs.Systems.Physics
{
    /// <summary>
    /// System which tries to replicate ABTA actor direction behavior
    /// </summary>
    [RequireEntityWith(
        typeof(AngularPositionComponent),
        typeof(AngularPositionTargetComponent),
        typeof(AngularThrustComponent))]

    public class DirectionSystemVanilla : UpdatableMatchingSystemBase<DirectionSystemVanilla>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Internal Constructors

        internal DirectionSystemVanilla(
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
            var angularPos = entity.Get<AngularPositionComponent>();
            var angularVel = entity.Get<AngularPositionTargetComponent>();

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
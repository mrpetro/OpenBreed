using OpenBreed.Core.Extensions;
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
        typeof(AngularPositionTargetComponent),
        typeof(AngularThrustComponent))]

    public class DirectionSystemVanilla : UpdatableSystemBase<DirectionSystemVanilla>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Internal Constructors

        internal DirectionSystemVanilla(
            IWorld world,
            IEntityMan entityMan) :
            base(world)
        {
            this.entityMan = entityMan;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var angularPos = entity.Get<AngularPositionComponent>();
            var angularVel = entity.Get<AngularPositionTargetComponent>();

            //Velocity equation
            //var newVel = angularVel.Value + angularThrust.Value * dt;

            var aPos = angularPos.Value;
            var dPos = angularVel.Value;

            var newPos = aPos.RotateTowards(dPos, (float)Math.PI * 0.125f, 1.0f);

            if (newPos == aPos)
                return;

            angularPos.Value = newPos;
            entity.RaiseEvent(new DirectionChangedEventArgs(angularPos.Value));
        }

        #endregion Protected Methods
    }
}
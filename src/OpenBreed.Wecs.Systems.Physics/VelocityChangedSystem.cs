﻿using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(typeof(VelocityComponent))]
    public class VelocityChangedSystem : UpdatableSystemBase<VelocityChangedSystem>
    {
        #region Private Fields

        private const float FLOOR_FRICTION = 0.2f;

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Internal Constructors

        internal VelocityChangedSystem(
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
            var velocity = entity.Get<VelocityComponent>();
            entity.RaiseEvent(new VelocityChangedEventArgs(velocity.Value));
        }

        #endregion Protected Methods
    }
}

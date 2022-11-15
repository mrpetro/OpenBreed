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
    public class VelocityChangedSystem : UpdatableSystemBase
    {
        #region Private Fields

        private const float FLOOR_FRICTION = 0.2f;

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Internal Constructors

        internal VelocityChangedSystem(IEntityMan entityMan)
        {
            this.entityMan = entityMan;

            RequireEntityWith<VelocityComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            var velocity = entity.Get<VelocityComponent>();
            entity.RaiseEvent(new VelocityChangedEventArgs(velocity.Value));
        }

        #endregion Protected Methods
    }
}

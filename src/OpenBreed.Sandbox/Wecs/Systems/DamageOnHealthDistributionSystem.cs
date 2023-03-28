using OpenBreed.Input.Interface;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    [RequireEntityWith(
        typeof(DamagerComponent))]
    internal class DamageOnHealthDistributionSystem : UpdatableSystemBase<DamageOnHealthDistributionSystem>
    {
        private readonly IEntityMan entityMan;

        internal DamageOnHealthDistributionSystem(
            IWorld world,
            IEntityMan entityMan)
        {
            this.entityMan = entityMan;
        }

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var damageComponent = entity.Get<DamagerComponent>();
        }
    }
}

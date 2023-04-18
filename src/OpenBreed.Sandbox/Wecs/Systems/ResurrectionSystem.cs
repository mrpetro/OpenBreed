using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
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
        typeof(HealthComponent),
        typeof(LivesComponent))]
    public class ResurrectionSystem : UpdatableSystemBase<ResurrectionSystem>
    {
        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {

        }
    }
}

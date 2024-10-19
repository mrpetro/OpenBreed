using OpenBreed.Core.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(ResurrectCommandComponent),
        typeof(ResurrectableComponent))]
    public class ResurrectionSystem : UpdatableMatchingSystemBase<ResurrectionSystem>
    {
        private readonly IWorldMan worldMan;

        public ResurrectionSystem(IWorldMan worldMan)
        {
            this.worldMan = worldMan;
        }

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            try
            {
                var resurectableCmp = entity.Get<ResurrectableComponent>();

                worldMan.RequestAddEntity(entity, resurectableCmp.WorldId);
            }
            finally
            {
                entity.Remove<ResurrectCommandComponent>();
            }
        }
    }
}

using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    public class TurretTargetingSystem : EventSystem<PositionChangedEvent, TurretTargetingSystem>
    {
        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly ILogger logger;

        public TurretTargetingSystem(
            IEventsMan eventsMan,
            IEntityMan entityMan,
            IWorldMan worldMan,
            ILogger logger) : base(eventsMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.logger = logger;
        }

        public override void Update(object sender, PositionChangedEvent e)
        {
            var entity = entityMan.GetById(e.EntityId);
            var entityPos = entity.GetPosition();

            var world = worldMan.GetById(entity.WorldId);

            var turrets = world.Entities.Where(entity => entity.Contains<TurretTargetComponent>());

            foreach (var turret in turrets)
            {
                var ttc = turret.Get<TurretTargetComponent>();

                if (ttc.TargetEntityId != e.EntityId)
                {
                    continue;
                }

                turret.SetTargetDirectionToCoordinates(entityPos);
            }


            logger.Verbose($"Entity {e.EntityId} position changed");

            //throw new NotImplementedException();
        }
    }
}

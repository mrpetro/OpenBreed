using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Events;
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
        private readonly ILogger logger;

        public TurretTargetingSystem(
            IEventsMan eventsMan,
            IEntityMan entityMan,
            ILogger logger) : base(eventsMan)
        {
            this.entityMan = entityMan;
            this.logger = logger;
        }

        public override void Update(object sender, PositionChangedEvent e)
        {
            logger.Verbose($"Entity {e.EntityId} position changed");

            //throw new NotImplementedException();
        }
    }
}

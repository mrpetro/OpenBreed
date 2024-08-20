using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Entities;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    public class TurretTrackUnlockingSystem : EntityEventSystem<TurretTrackUnlockingSystem, ContactEndedEvent>
    {
        private readonly IFixtureMan fixtureMan;
        private readonly IEntityMan entityMan;

        public TurretTrackUnlockingSystem(
            IEventsMan eventsMan,
            IEntityMan entityMan,
            IFixtureMan fixtureMan) : base(eventsMan)
        {
            this.entityMan = entityMan;
            this.fixtureMan = fixtureMan;
        }

        public override void Update(ContactEndedEvent e)
        {
            var entity = entityMan.GetById(e.EntityId);
            var tc = entity.TryGet<TrackingComponent>();

            if (tc is null)
            {
                return;
            }

            var trackingFixture = fixtureMan.GetById(e.EntityFixtureId);

            if (!trackingFixture.GroupIds.Any(g => g == ColliderTypes.ActorSight))
            {
                return;
            }

            var tracedFixture = fixtureMan.GetById(e.ContactedFixtureId);

            if (!tracedFixture.GroupIds.Any(g => g == ColliderTypes.ActorBody))
            {
                return;
            }

            tc.EntityId = -1;
        }
    }

    public class TurretTrackLockingSystem : EntityEventSystem<TurretTrackLockingSystem, ContactStartedEvent>
    {
        private readonly IFixtureMan fixtureMan;
        private readonly IEntityMan entityMan;

        public TurretTrackLockingSystem(
            IEventsMan eventsMan,
            IEntityMan entityMan,
            IFixtureMan fixtureMan) : base(eventsMan)
        {
            this.entityMan = entityMan;
            this.fixtureMan = fixtureMan;
        }

        public override void Update(ContactStartedEvent e)
        {
            var entity = entityMan.GetById(e.EntityId);
            var tc = entity.TryGet<TrackingComponent>();

            if(tc is null)
            {
                return;
            }

            var trackingFixture = fixtureMan.GetById(e.EntityFixtureId);

            if (!trackingFixture.GroupIds.Any(g => g == ColliderTypes.ActorSight))
            {
                return;
            }

            var tracedFixture = fixtureMan.GetById(e.ContactedFixtureId);

            if (!tracedFixture.GroupIds.Any(g => g == ColliderTypes.ActorBody))
            {
                return;
            }

            tc.EntityId = e.ContactedEntityId;


        }
    }


    public class TurretTrackingSystem : IUpdatableSystem
    {
        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;

        public TurretTrackingSystem(
            IWorldMan worldMan,
            IEntityMan entityMan)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
        }

        public void Update(IUpdateContext context)
        {
            var entities = entityMan.Where(entity => entity.Contains<TrackingComponent>());

            foreach (var entity in entities)
            {
                var tc = entity.Get<TrackingComponent>();

                if (tc.EntityId == -1)
                {
                    continue;
                }

                var trackedEntity = entityMan.GetById(tc.EntityId);
                var trackedPosition = trackedEntity.GetPosition();
                entity.SetTargetDirectionToCoordinates(trackedPosition);
            }
        }
    }
}

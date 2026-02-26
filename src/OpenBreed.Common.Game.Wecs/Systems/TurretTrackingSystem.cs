using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Physics.Systems.Events;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    public class TurretTrackUnlockingSystem : IEventSystem<ContactEndedEvent>
    {
        private readonly IFixtureMan fixtureMan;
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        public TurretTrackUnlockingSystem(
            IEntityMan entityMan,
            IEventsMan eventsMan,
            IFixtureMan fixtureMan)
        {
            this.entityMan = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.fixtureMan = fixtureMan ?? throw new ArgumentNullException(nameof(fixtureMan));
        }

        public void Update(ContactEndedEvent e)
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

            if (tc.EntityId != -1)
            {
                tc.EntityId = -1;
                eventsMan.Raise(new TrackingTargetChangedEvent(e.EntityId, tc.EntityId));
            }
        }


    }

    public class TurretTrackLockingSystem : IEventSystem<ContactStartedEvent>
    {
        private readonly IFixtureMan fixtureMan;
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        public TurretTrackLockingSystem(
            IEntityMan entityMan,
            IEventsMan eventsMan,
            IFixtureMan fixtureMan)
        {
            this.entityMan = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.fixtureMan = fixtureMan ?? throw new ArgumentNullException(nameof(fixtureMan));
        }

        public void Update(ContactStartedEvent e)
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

            if (tc.EntityId != e.ContactedEntityId)
            {
                tc.EntityId = e.ContactedEntityId;
                eventsMan.Raise(new TrackingTargetChangedEvent(e.EntityId, tc.EntityId));
            }
        }
    }

    [RequireEntityWith(typeof(TrackingComponent))]
    public class TurretTrackingSystem : IUpdatableSystem
    {
        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        public TurretTrackingSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan)
        {
            this.worldMan = worldMan ?? throw new ArgumentNullException(nameof(worldMan));
            this.entityMan = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
        }

        public void Update(IEnumerable<IEntity> entities, IUpdateContext context)
        {
            foreach (var entity in entities)
            {
                var tc = entity.Get<TrackingComponent>();

                if (tc.EntityId == -1)
                {
                    continue;
                }

                eventsMan.Raise(new TrackingTargetEvent(entity.Id, tc.EntityId));
                

            }
        }
    }
}

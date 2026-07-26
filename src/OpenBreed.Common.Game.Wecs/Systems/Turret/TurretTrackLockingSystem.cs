using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Physics.Systems.Events;

namespace OpenBreed.Common.Game.Wecs.Systems.Turret
{
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

        public void OnEvent(ContactStartedEvent e, IWorld world)
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
}

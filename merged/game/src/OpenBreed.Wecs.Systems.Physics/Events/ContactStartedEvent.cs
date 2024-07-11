using OpenBreed.Wecs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Events
{
    public class ContactStartedEvent : EntityEvent
    {
        #region Public Constructors

        public ContactStartedEvent(
            int entityId,
            int entityFixtureId,
            int contactedEntityId,
            int contactedFixtureId) : base(entityId)
        {
            EntityFixtureId = entityFixtureId;
            ContactedEntityId = contactedEntityId;
            ContactedFixtureId = contactedFixtureId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int ContactedEntityId { get; }
        public int ContactedFixtureId { get; }
        public int EntityFixtureId { get; }

        #endregion Public Properties
    }
}
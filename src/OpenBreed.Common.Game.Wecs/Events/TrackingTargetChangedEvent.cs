using OpenBreed.Wecs.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Events
{
    public class TrackingTargetChangedEvent : EntityEvent
    {
        #region Public Constructors

        public TrackingTargetChangedEvent(int entityId, int trackedEntityId) : base(entityId)
        {
            TrackedEntityId = trackedEntityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TrackedEntityId { get; }

        #endregion Public Properties
    }
}
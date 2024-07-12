using OpenBreed.Wecs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Events
{
    public class ActorCollisionEvent : EntityEvent
    {
        #region Public Constructors

        public ActorCollisionEvent(int entityId, int otherEntityId)
            : base(entityId)
        {
            OtherEntityId = otherEntityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int OtherEntityId { get; }

        #endregion Public Properties
    }
}

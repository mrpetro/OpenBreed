using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Events
{
    public abstract class EntitiesEvent : EventArgs
    {
        #region Protected Constructors

        protected EntitiesEvent(int[] entityIds)
        {
            EntityIds = entityIds;
        }

        #endregion Protected Constructors

        #region Public Properties

        public int[] EntityIds { get; }

        #endregion Public Properties
    }

    public abstract class EntityEvent : EventArgs
    {
        #region Protected Constructors

        protected EntityEvent(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Protected Constructors

        #region Public Properties

        public int EntityId { get; }

        #endregion Public Properties
    }
}

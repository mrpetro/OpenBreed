using OpenBreed.Wecs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Events
{    
    /// <summary>  
    /// Event fired when entity is leaving the world  
    /// </summary>
    public class EntityLeavingEvent : EntityEvent
    {
        #region Public Constructors

        public EntityLeavingEvent(int entityId)
            : base(entityId)
        {
        }

        #endregion Public Constructors
    }
}

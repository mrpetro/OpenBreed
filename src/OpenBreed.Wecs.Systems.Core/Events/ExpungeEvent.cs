using OpenBreed.Wecs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Core.Events
{    
    /// <summary>  
    /// Event fired when entity is expunged from world  
    /// </summary>
    public class ExpungeEvent : EntityEvent
    {
        #region Public Constructors

        public ExpungeEvent(int entityId)
            : base(entityId)
        {
        }

        #endregion Public Constructors
    }
}

using OpenBreed.Wecs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Core.Events
{
    /// <summary>
    /// Event fired when entity emits another entity
    /// </summary>
    public class EmitEntityEvent : EntityEvent
    {
        #region Public Constructors

        public EmitEntityEvent(int entityId, int emiterEntityId)
            : base(entityId)
        {
            EmiterEntityId = emiterEntityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EmiterEntityId { get; }

        #endregion Public Properties
    }

}

using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Worlds;
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

        public EntityLeavingEvent(int entityId, int worldId)
            : base(entityId)
        {
            WorldId = worldId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }

        #endregion Public Properties
    }
}
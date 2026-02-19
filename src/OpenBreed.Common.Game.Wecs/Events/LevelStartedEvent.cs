using OpenBreed.Wecs.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Events
{
    public class LevelStartedEvent : EntityEvent
    {
        public LevelStartedEvent(int entityId) : base(entityId)
        {
        }
    }
}

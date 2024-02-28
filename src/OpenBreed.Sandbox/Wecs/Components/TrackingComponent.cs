using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public class TrackingComponent : IEntityComponent
    {
        public TrackingComponent(int entityId)
        {
            EntityId = entityId;
        }

        public int EntityId { get; set; }
    }
}

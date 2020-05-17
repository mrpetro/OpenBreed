using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Common.Components
{
    public class WorldComponent : IEntityComponent
    {
        internal WorldComponent(WorldComponentBuilder builder)
        {
            WorldId = builder.WorldId;
        }

        public int WorldId { get; set; }
    }
}

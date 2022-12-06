using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
{
    public static class WorldBuilderExtensions
    {
        public static void AddSequenceUpdateSystem<TSystem>(this WorldBuilder builder) where TSystem : IEntityUpdateSystem
        {
            builder.AddSystem<SequenceUpdateSystem>((sf, world) => new SequenceUpdateSystem(world, (TSystem)sf.Create<TSystem>(world)));
        }
    }
}

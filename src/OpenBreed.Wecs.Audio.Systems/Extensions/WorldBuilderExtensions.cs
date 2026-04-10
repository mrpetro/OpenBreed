using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Audio.Systems.Extensions
{
    public static class WorldBuilderExtensions
    {
        public static IWorldBuilder AddSoundSystems(this IWorldBuilder builder)
        {
            builder.AddSystem<SoundSystem>();

            return builder;
        }
    }
}

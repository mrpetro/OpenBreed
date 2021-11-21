using OpenBreed.Wecs.Components.Audio;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Audio.Extensions
{
    public static class EntityExtensions
    {
        public static void EmitSound(this Entity entity, int soundId)
        {
            entity.Set(new SoundPlayerComponent(soundId));
        }
    }
}

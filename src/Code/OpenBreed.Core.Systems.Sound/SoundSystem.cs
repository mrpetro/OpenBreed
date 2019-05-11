using OpenBreed.Core.Systems.Sound.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems.Sound
{
    public class SoundSystem : WorldSystem<ISoundComponent>, ISoundSystem
    {
        protected override void AddComponent(ISoundComponent component)
        {
            throw new NotImplementedException();
        }

        protected override void RemoveComponent(ISoundComponent component)
        {
            throw new NotImplementedException();
        }
    }
}

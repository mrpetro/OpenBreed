using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Components.Audio.Xml;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Audio.Extensions
{
    public static class EntityFactoryExtensions
    {
        public static void SetupAudioComponents(this IEntityFactory entityFactory, IServiceProvider serviceProvider)
        {
            entityFactory.RegisterComponentFactory<XmlSoundPlayerComponent>(serviceProvider.GetService<SoundPlayerComponentFactory>());
        }
    }
}

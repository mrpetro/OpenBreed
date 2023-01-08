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
    public static class ComponentFactoryProviderExtensions
    {
        public static void SetupXmlAudioComponents(this IComponentFactoryProvider provider, IServiceProvider serviceProvider)
        {
            provider.RegisterComponentFactory<XmlSoundPlayerComponent>(serviceProvider.GetService<SoundPlayerComponentFactory>());
        }
    }
}

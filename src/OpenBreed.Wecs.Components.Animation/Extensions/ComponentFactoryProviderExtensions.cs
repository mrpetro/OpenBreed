using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Components.Animation.Xml;

namespace OpenBreed.Wecs.Components.Animation.Extensions
{
    public static class ComponentFactoryProviderExtensions
    {
        public static void SetupAnimationComponents(this IComponentFactoryProvider provider, IServiceProvider serviceProvider)
        {
            provider.RegisterComponentFactory<XmlAnimationComponent>(serviceProvider.GetService<AnimationComponentFactory>());
        }
    }
}

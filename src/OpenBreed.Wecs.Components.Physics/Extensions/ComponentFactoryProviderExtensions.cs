using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Components.Physics.Xml;
using OpenBreed.Wecs.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Physics.Extensions
{
    public static class ComponentFactoryProviderExtensions
    {
        public static void SetupPhysicsComponents(this IComponentFactoryProvider provider, IServiceProvider serviceProvider)
        {
            provider.RegisterComponentFactory<XmlBodyComponent>(serviceProvider.GetService<BodyComponentFactory>());
            provider.RegisterComponentFactory<XmlMotionComponent>(serviceProvider.GetService<MotionComponentFactory>());
        }
    }
}

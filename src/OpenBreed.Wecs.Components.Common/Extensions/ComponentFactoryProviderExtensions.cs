using OpenBreed.Common;
using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace OpenBreed.Wecs.Components.Common.Extensions
{
    public static class ComponentFactoryProviderExtensions
    {
        public static void SetupCommonComponents(this IComponentFactoryProvider provider, IServiceProvider serviceProvider)
        {
            provider.RegisterComponentFactory<XmlPositionComponent>(serviceProvider.GetService<PositionComponentFactory>());
            provider.RegisterComponentFactory<XmlGridPositionComponent>(serviceProvider.GetService<GridPositionComponentFactory>());
            provider.RegisterComponentFactory<XmlMetadataComponent>(serviceProvider.GetService<MetadataComponentFactory>());
            provider.RegisterComponentFactory<XmlThrustComponent>(serviceProvider.GetService<ThrustComponentFactory>());
            provider.RegisterComponentFactory<XmlVelocityComponent>(serviceProvider.GetService<VelocityComponentFactory>());
            provider.RegisterComponentFactory<XmlTimerComponent>(serviceProvider.GetService<TimerComponentFactory>());
            provider.RegisterComponentFactory<XmlAngularPositionComponent>(serviceProvider.GetService<AngularPositionComponentFactory>());
            provider.RegisterComponentFactory<XmlAngularVelocityComponent>(serviceProvider.GetService<AngularVelocityComponentFactory>());
            provider.RegisterComponentFactory<XmlAngularThrustComponent>(serviceProvider.GetService<AngularThrustComponentFactory>());
            provider.RegisterComponentFactory<XmlMessagingComponent>(serviceProvider.GetService<MessagingComponentFactory>());
            provider.RegisterComponentFactory<XmlFollowedComponent>(serviceProvider.GetService<FollowedComponentFactory>());
        }
    }
}

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
    public static class EntityFactoryExtensions
    {
        public static void SetupCommonComponents(this IEntityFactory entityFactory, IServiceProvider serviceProvider)
        {
            entityFactory.RegisterComponentFactory<XmlPositionComponent>(serviceProvider.GetService<PositionComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlGridPositionComponent>(serviceProvider.GetService<GridPositionComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlMetadataComponent>(serviceProvider.GetService<MetadataComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlThrustComponent>(serviceProvider.GetService<ThrustComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlVelocityComponent>(serviceProvider.GetService<VelocityComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlTimerComponent>(serviceProvider.GetService<TimerComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlAngularPositionComponent>(serviceProvider.GetService<AngularPositionComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlAngularVelocityComponent>(serviceProvider.GetService<AngularVelocityComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlAngularThrustComponent>(serviceProvider.GetService<AngularThrustComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlMessagingComponent>(serviceProvider.GetService<MessagingComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlFollowedComponent>(serviceProvider.GetService<FollowedComponentFactory>());
        }
    }
}

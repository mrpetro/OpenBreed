using OpenBreed.Common;
using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Common.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupCommonComponents(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<PositionComponentFactory>(() => new PositionComponentFactory());
            manCollection.AddSingleton<GridPositionComponentFactory>(() => new GridPositionComponentFactory());
            manCollection.AddSingleton<VelocityComponentFactory>(() => new VelocityComponentFactory());
            manCollection.AddSingleton<ThrustComponentFactory>(() => new ThrustComponentFactory());
            manCollection.AddSingleton<TimerComponentFactory>(() => new TimerComponentFactory());
            manCollection.AddSingleton<MetadataComponentFactory>(() => new MetadataComponentFactory());
            manCollection.AddSingleton<AngularPositionComponentFactory>(() => new AngularPositionComponentFactory());
            manCollection.AddSingleton<AngularVelocityComponentFactory>(() => new AngularVelocityComponentFactory());
            manCollection.AddSingleton<AngularThrustComponentFactory>(() => new AngularThrustComponentFactory());
            manCollection.AddSingleton<MessagingComponentFactory>(() => new MessagingComponentFactory());
            manCollection.AddSingleton<FollowedComponentFactory>(() => new FollowedComponentFactory());

            var entityFactory = manCollection.GetManager<IEntityFactory>();
            entityFactory.RegisterComponentFactory<XmlPositionComponent>(manCollection.GetManager<PositionComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlGridPositionComponent>(manCollection.GetManager<GridPositionComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlMetadataComponent>(manCollection.GetManager<MetadataComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlThrustComponent>(manCollection.GetManager<ThrustComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlVelocityComponent>(manCollection.GetManager<VelocityComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlTimerComponent>(manCollection.GetManager<TimerComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlAngularPositionComponent>(manCollection.GetManager<AngularPositionComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlAngularVelocityComponent>(manCollection.GetManager<AngularVelocityComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlAngularThrustComponent>(manCollection.GetManager<AngularThrustComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlMessagingComponent>(manCollection.GetManager<MessagingComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlFollowedComponent>(manCollection.GetManager<FollowedComponentFactory>());
        }
    }
}

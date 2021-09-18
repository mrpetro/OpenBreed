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
            XmlComponentsList.RegisterComponentType<XmlPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlGridPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlClassComponent>();
            XmlComponentsList.RegisterComponentType<XmlThrustComponent>();
            XmlComponentsList.RegisterComponentType<XmlVelocityComponent>();
            XmlComponentsList.RegisterComponentType<XmlTimerComponent>();
            XmlComponentsList.RegisterComponentType<XmlAngularPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlMessagingComponent>();

            manCollection.AddSingleton<PositionComponentFactory>(() => new PositionComponentFactory());
            manCollection.AddSingleton<GridPositionComponentFactory>(() => new GridPositionComponentFactory());
            manCollection.AddSingleton<VelocityComponentFactory>(() => new VelocityComponentFactory());
            manCollection.AddSingleton<ThrustComponentFactory>(() => new ThrustComponentFactory());
            manCollection.AddSingleton<TimerComponentFactory>(() => new TimerComponentFactory());
            manCollection.AddSingleton<ClassComponentFactory>(() => new ClassComponentFactory());
            manCollection.AddSingleton<AngularPositionComponentFactory>(() => new AngularPositionComponentFactory());
            manCollection.AddSingleton<MessagingComponentFactory>(() => new MessagingComponentFactory());

            var entityFactory = manCollection.GetManager<IEntityFactory>();
            entityFactory.RegisterComponentFactory<XmlPositionComponent>(manCollection.GetManager<PositionComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlGridPositionComponent>(manCollection.GetManager<GridPositionComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlClassComponent>(manCollection.GetManager<ClassComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlThrustComponent>(manCollection.GetManager<ThrustComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlVelocityComponent>(manCollection.GetManager<VelocityComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlTimerComponent>(manCollection.GetManager<TimerComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlAngularPositionComponent>(manCollection.GetManager<AngularPositionComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlMessagingComponent>(manCollection.GetManager<MessagingComponentFactory>());
        }
    }
}

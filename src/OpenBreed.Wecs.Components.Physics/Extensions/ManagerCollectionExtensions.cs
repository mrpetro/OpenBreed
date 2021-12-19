using OpenBreed.Common;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Physics.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Physics.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupPhysicsComponents(this IManagerCollection manCollection)
        {
            XmlComponentsList.RegisterComponentType<XmlBodyComponent>();
            XmlComponentsList.RegisterComponentType<XmlMotionComponent>();

            var builderFactory = manCollection.GetManager<IBuilderFactory>();

            builderFactory.Register<BodyComponentBuilder>(() => new BodyComponentBuilder(manCollection.GetManager<IShapeMan>(),
                                                                                         manCollection.GetManager<ICollisionMan<Entity>>()));

            manCollection.AddSingleton<BodyComponentFactory>(() => new BodyComponentFactory(builderFactory));
            manCollection.AddSingleton<MotionComponentFactory>(() => new MotionComponentFactory());


            var entityFactory = manCollection.GetManager<IEntityFactory>();
            entityFactory.RegisterComponentFactory<XmlBodyComponent>(manCollection.GetManager<BodyComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlMotionComponent>(manCollection.GetManager<MotionComponentFactory>());
        }
    }
}

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
    public static class EntityFactoryExtensions
    {
        public static void SetupPhysicsComponents(this IEntityFactory entityFactory, IServiceProvider serviceProvider)
        {
            entityFactory.RegisterComponentFactory<XmlBodyComponent>(serviceProvider.GetService<BodyComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlMotionComponent>(serviceProvider.GetService<MotionComponentFactory>());
        }
    }
}

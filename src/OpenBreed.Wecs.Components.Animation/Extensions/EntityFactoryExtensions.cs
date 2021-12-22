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
    public static class EntityFactoryExtensions
    {
        public static void SetupAnimationComponents(this IEntityFactory entityFactory, IServiceProvider serviceProvider)
        {
            entityFactory.RegisterComponentFactory<XmlAnimationComponent>(serviceProvider.GetService<AnimationComponentFactory>());
        }
    }
}

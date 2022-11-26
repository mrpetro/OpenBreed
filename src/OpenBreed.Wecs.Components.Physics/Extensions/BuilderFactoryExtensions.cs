using OpenBreed.Common;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Components.Physics.Builders;

namespace OpenBreed.Wecs.Components.Physics.Extensions
{
    public static class BuilderFactoryExtensions
    {
        public static void SetupPhysicsBuilderFactories(this IBuilderFactory builderFactory, IServiceProvider serviceProvider)
        {
            builderFactory.Register<BodyComponentBuilder>(() => new BodyComponentBuilder(
                serviceProvider.GetService<IShapeMan>(),                                                                         
                serviceProvider.GetService<ICollisionMan<IEntity>>()));

            builderFactory.Register<BodyFixtureBuilder>(() => new BodyFixtureBuilder(
                serviceProvider.GetService<IFixtureMan>(),
                serviceProvider.GetService<IShapeMan>(),                                                                
                serviceProvider.GetService<ICollisionMan<IEntity>>()));
        }
    }
}

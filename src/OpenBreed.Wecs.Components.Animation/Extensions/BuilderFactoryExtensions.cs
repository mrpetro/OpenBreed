using OpenBreed.Common;
using OpenBreed.Wecs.Entities;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Interface;

namespace OpenBreed.Wecs.Components.Animation.Extensions
{
    public static class BuilderFactoryExtensions
    {
        public static void SetupAnimationBuilders(this IBuilderFactory builderFactory, IServiceProvider serviceProvider)
        {
            builderFactory.Register<AnimationComponentBuilder>(() => new AnimationComponentBuilder(serviceProvider.GetService<IClipMan<IEntity>>()));
        }
    }
}

using OpenBreed.Common;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Interface;
using OpenBreed.Animation.Interface;

namespace OpenBreed.Wecs.Control.Components.Extensions
{
    public static class BuilderFactoryExtensions
    {
        public static void SetupWecsControlBuilders(this IBuilderFactory builderFactory, IServiceProvider serviceProvider)
        {
            builderFactory.Register<AnimationComponentBuilder>(() => new AnimationComponentBuilder(serviceProvider.GetService<IClipMan<IEntity>>()));
        }
    }
}

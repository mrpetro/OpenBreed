using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Wecs.Components.Rendering.Extensions
{
    public static class BuilderFactoryExtensions
    {
        public static void SetupRenderingComponents(this IBuilderFactory builderFactory, IServiceProvider serviceProvider)
        {
            builderFactory.Register<SpriteComponentBuilder>(() => new SpriteComponentBuilder(serviceProvider.GetService<ISpriteMan>()));
            builderFactory.Register<TextComponentBuilder>(() => new TextComponentBuilder(serviceProvider.GetService<IFontMan>()));
            builderFactory.Register<CameraComponentBuilder>(() => new CameraComponentBuilder());
            builderFactory.Register<ViewportComponentBuilder>(() => new ViewportComponentBuilder());
            builderFactory.Register<TilePutterComponentBuilder>(() => new TilePutterComponentBuilder(serviceProvider.GetService<ITileMan>()));
        }
    }
}

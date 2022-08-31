using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Components.Rendering.Xml;

namespace OpenBreed.Wecs.Components.Rendering.Extensions
{
    public static class ComponentFactoryProviderExtensions
    {
        public static void SetupRenderingComponents(this IComponentFactoryProvider provider, IServiceProvider serviceProvider)
        {
            provider.RegisterComponentFactory<XmlSpriteComponent>(serviceProvider.GetService<SpriteComponentFactory>());
            provider.RegisterComponentFactory<XmlTextComponent>(serviceProvider.GetService<TextComponentFactory>());
            provider.RegisterComponentFactory<XmlViewportComponent>(serviceProvider.GetService<ViewportComponentFactory>());
            provider.RegisterComponentFactory<XmlCameraComponent>(serviceProvider.GetService<CameraComponentFactory>());
            provider.RegisterComponentFactory<XmlTilePutterComponent>(serviceProvider.GetService<TilePutterComponentFactory>());
        }
    }
}

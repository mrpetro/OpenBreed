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
    public static class EntityFactoryExtensions
    {
        public static void SetupRenderingComponents(this IEntityFactory entityFactory, IServiceProvider serviceProvider)
        {
            entityFactory.RegisterComponentFactory<XmlSpriteComponent>(serviceProvider.GetService<SpriteComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlTextComponent>(serviceProvider.GetService<TextComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlViewportComponent>(serviceProvider.GetService<ViewportComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlCameraComponent>(serviceProvider.GetService<CameraComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlTilePutterComponent>(serviceProvider.GetService<TilePutterComponentFactory>());
        }
    }
}

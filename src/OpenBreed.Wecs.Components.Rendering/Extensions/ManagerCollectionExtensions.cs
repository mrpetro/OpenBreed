using OpenBreed.Common;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Rendering.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Rendering.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupRenderingComponents(this IManagerCollection manCollection)
        {
            XmlComponentsList.RegisterComponentType<XmlSpriteComponent>();
            XmlComponentsList.RegisterComponentType<XmlTextComponent>();
            XmlComponentsList.RegisterComponentType<XmlViewportComponent>();

            manCollection.AddTransient<SpriteComponentBuilder>(() => new SpriteComponentBuilder(manCollection.GetManager<ISpriteMan>()));
            manCollection.AddTransient<TextComponentBuilder>(() => new TextComponentBuilder(manCollection.GetManager<IFontMan>()));
            manCollection.AddTransient<CameraComponentBuilder>(() => new CameraComponentBuilder());
            manCollection.AddTransient<ViewportComponentBuilder>(() => new ViewportComponentBuilder());

            manCollection.AddSingleton<SpriteComponentFactory>(() => new SpriteComponentFactory(manCollection));
            manCollection.AddSingleton<ViewportComponentFactory>(() => new ViewportComponentFactory(manCollection));
            manCollection.AddSingleton<TextComponentFactory>(() => new TextComponentFactory(manCollection));
            manCollection.AddSingleton<CameraComponentFactory>(() => new CameraComponentFactory(manCollection));

            var entityFactory = manCollection.GetManager<IEntityFactory>();
            entityFactory.RegisterComponentFactory<XmlSpriteComponent>(manCollection.GetManager<SpriteComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlTextComponent>(manCollection.GetManager<TextComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlViewportComponent>(manCollection.GetManager<ViewportComponentFactory>());
        }
    }
}

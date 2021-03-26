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

            manCollection.AddTransient<SpriteComponentBuilder>(() => new SpriteComponentBuilder(manCollection.GetManager<ISpriteMan>()));
            manCollection.AddTransient<TextComponentBuilder>(() => new TextComponentBuilder(manCollection.GetManager<IFontMan>()));

            manCollection.AddSingleton<SpriteComponentFactory>(() => new SpriteComponentFactory(manCollection));
            manCollection.AddSingleton<TextComponentFactory>(() => new TextComponentFactory(manCollection));

            var entityFactory = manCollection.GetManager<IEntityFactory>();
            entityFactory.RegisterComponentFactory<XmlSpriteComponent>(manCollection.GetManager<SpriteComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlTextComponent>(manCollection.GetManager<TextComponentFactory>());
        }
    }
}

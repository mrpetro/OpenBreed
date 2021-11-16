﻿using OpenBreed.Common;
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
            XmlComponentsList.RegisterComponentType<XmlCameraComponent>();
            XmlComponentsList.RegisterComponentType<XmlTilePutterComponent>();

            var builderFactory = manCollection.GetManager<IBuilderFactory>();

            builderFactory.Register<SpriteComponentBuilder>(() => new SpriteComponentBuilder(manCollection.GetManager<ISpriteMan>()));
            builderFactory.Register<TextComponentBuilder>(() => new TextComponentBuilder(manCollection.GetManager<IFontMan>()));
            builderFactory.Register<CameraComponentBuilder>(() => new CameraComponentBuilder());
            builderFactory.Register<ViewportComponentBuilder>(() => new ViewportComponentBuilder());
            builderFactory.Register<TilePutterComponentBuilder>(() => new TilePutterComponentBuilder(manCollection.GetManager<ITileMan>()));

            manCollection.AddSingleton<SpriteComponentFactory>(() => new SpriteComponentFactory(manCollection.GetManager<IBuilderFactory>()));
            manCollection.AddSingleton<ViewportComponentFactory>(() => new ViewportComponentFactory(manCollection.GetManager<IBuilderFactory>()));
            manCollection.AddSingleton<TextComponentFactory>(() => new TextComponentFactory(manCollection.GetManager<IBuilderFactory>()));
            manCollection.AddSingleton<CameraComponentFactory>(() => new CameraComponentFactory(manCollection.GetManager<IBuilderFactory>()));
            manCollection.AddSingleton<TilePutterComponentFactory>(() => new TilePutterComponentFactory(manCollection.GetManager<IBuilderFactory>()));

            var entityFactory = manCollection.GetManager<IEntityFactory>();
            entityFactory.RegisterComponentFactory<XmlSpriteComponent>(manCollection.GetManager<SpriteComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlTextComponent>(manCollection.GetManager<TextComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlViewportComponent>(manCollection.GetManager<ViewportComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlCameraComponent>(manCollection.GetManager<CameraComponentFactory>());
            entityFactory.RegisterComponentFactory<XmlTilePutterComponent>(manCollection.GetManager<TilePutterComponentFactory>());
        }
    }
}

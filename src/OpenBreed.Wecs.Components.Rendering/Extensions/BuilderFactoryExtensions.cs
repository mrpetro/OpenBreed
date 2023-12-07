using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Common.Interface;
using OpenBreed.Rendering.Interface.Data;

namespace OpenBreed.Wecs.Components.Rendering.Extensions
{
    public static class BuilderFactoryExtensions
    {
        public static void SetupRenderingComponents(this IBuilderFactory builderFactory, IServiceProvider sp)
        {
            var dataLoderFactory = sp.GetService<IDataLoaderFactory>();

            builderFactory.Register<PictureComponentBuilder>(
                () => new PictureComponentBuilder(
                    dataLoderFactory.GetLoader<IPictureDataLoader>()));
            builderFactory.Register<SpriteComponentBuilder>(
                () => new SpriteComponentBuilder(sp.GetService<ISpriteMan>()));
            builderFactory.Register<TextComponentBuilder>(
                () => new TextComponentBuilder(sp.GetService<IFontMan>()));
            builderFactory.Register<CameraComponentBuilder>(
                () => new CameraComponentBuilder());
            builderFactory.Register<ViewportComponentBuilder>(
                () => new ViewportComponentBuilder());
            builderFactory.Register<TilePutterComponentBuilder>(
                () => new TilePutterComponentBuilder(sp.GetService<ITileMan>()));
            builderFactory.Register<TileGridComponentBuilder>(
                () => new TileGridComponentBuilder(
                    sp.GetService<ITileMan>(),
                    sp.GetService<ITileGridFactory>()));
        }
    }
}

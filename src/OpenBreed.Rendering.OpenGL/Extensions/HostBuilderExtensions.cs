using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Data;
using OpenBreed.Rendering.OpenGL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupOpenGLManagers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ITextureMan, TextureMan>();
                services.AddSingleton<ITileMan, TileMan>();
                services.AddSingleton<ITileGridFactory, TileGridFactory>();
                services.AddSingleton<IRenderableFactory, RenderableFactory>();
                services.AddSingleton<ITileGridFactory, TileGridFactory>();
                services.AddSingleton<SpriteMan, SpriteMan>();
                services.AddSingleton<ISpriteMan, SpriteMan>();
                services.AddSingleton<IStampMan, StampMan>();
                services.AddSingleton<IFontMan, FontMan>();
                services.AddSingleton<IPrimitiveRenderer, PrimitiveRenderer>();
                services.AddSingleton<ISpriteRenderer, SpriteRenderer>();
                services.AddSingleton<IRenderingMan, RenderingMan>();
            });
        }

        //public static void SetupSpriteSetDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        //{
        //    dataLoaderFactory.Register<ISpriteAtlasDataLoader>(() => new SpriteAtlasDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
        //                                                     managerCollection.GetManager<AssetsDataProvider>(),
        //                                                     managerCollection.GetManager<ITextureMan>(),
        //                                                     managerCollection.GetManager<ISpriteMan>()));
        //}

        //public static void SetupTileSetDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        //{
        //    dataLoaderFactory.Register<ITileAtlasDataLoader>(() => new TileAtlasDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
        //                                                     managerCollection.GetManager<AssetsDataProvider>(),
        //                                                     managerCollection.GetManager<ITextureMan>(),
        //                                                     managerCollection.GetManager<ITileMan>(),
        //                                                     managerCollection.GetManager<ILogger>()));
        //}

        //public static void SetupTileStampDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        //{
        //    dataLoaderFactory.Register<ITileStampDataLoader>(() => new TileStampDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
        //                                                     managerCollection.GetManager<AssetsDataProvider>(),
        //                                                     managerCollection.GetManager<ITextureMan>(),
        //                                                     managerCollection.GetManager<IStampMan>(),
        //                                                     managerCollection.GetManager<ITileMan>(),
        //                                                     managerCollection.GetManager<ILogger>()));
        //}
    }
}

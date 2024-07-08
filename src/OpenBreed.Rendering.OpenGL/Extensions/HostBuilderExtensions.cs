using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Model;
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
                services.AddSingleton<PictureMan, PictureMan>();
                services.AddSingleton<PaletteMan, PaletteMan>();
                services.AddSingleton<ISpriteMan>((sp) => sp.GetService<SpriteMan>());
                services.AddSingleton<IPictureMan>((sp) => sp.GetService<PictureMan>());
                services.AddSingleton<IPaletteMan>((sp) => sp.GetService<PaletteMan>());
                services.AddSingleton<IStampMan, StampMan>();
                services.AddSingleton<IFontMan, FontMan>();
                services.AddSingleton<IPrimitiveRenderer, PrimitiveRenderer>();
                services.AddSingleton<ISpriteRenderer, SpriteRenderer>();
                services.AddSingleton<IPictureRenderer, PictureRenderer>();
                services.AddSingleton<IRenderingMan, RenderingMan>();
            });
        }

        public static void SetupSpriteSetDataLoader(this DataLoaderFactory dataLoaderFactory, IServiceProvider sp)
        {
            dataLoaderFactory.Register<ISpriteAtlasDataLoader>(() => new SpriteAtlasDataLoader(
                sp.GetService<IRepositoryProvider>(),                                           
                sp.GetService<SpriteAtlasDataProvider>(),
                sp.GetService<ITextureMan>(),                                            
                sp.GetService<ISpriteMan>(),                                             
                sp.GetService<ISpriteMerger>()));
        }

        public static void SetupPictureDataLoader(this DataLoaderFactory dataLoaderFactory, IServiceProvider sp)
        {
            dataLoaderFactory.Register<IPictureDataLoader>(() => new PictureDataLoader(                                         
                sp.GetService<ImagesDataProvider>(),
                sp.GetService<ITextureMan>(),                                          
                sp.GetService<IPictureMan>()));
        }

        public static void SetupTileSetDataLoader(this DataLoaderFactory dataLoaderFactory, IServiceProvider sp)
        {
            dataLoaderFactory.Register<ITileAtlasDataLoader>(() => new TileAtlasDataLoader(
                sp.GetService<IRepositoryProvider>(),                                            
                sp.GetService<TileAtlasDataProvider>(),
                sp.GetService<ITextureMan>(),                                             
                sp.GetService<ITileMan>(),                                           
                sp.GetService<ILogger>()));
        }

        public static void SetupTileStampDataLoader(this DataLoaderFactory dataLoaderFactory, IServiceProvider sp)
        {
            dataLoaderFactory.Register<ITileStampDataLoader>(() => new TileStampDataLoader(
                sp.GetService<IRepositoryProvider>(),                                            
                sp.GetService<AssetsDataProvider>(),                                              
                sp.GetService<ITextureMan>(),                                           
                sp.GetService<IStampMan>(),                                              
                sp.GetService<ITileMan>(),                                             
                sp.GetService<ILogger>()));
        }
    }
}

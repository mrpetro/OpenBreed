using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Data;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupOpenGLManagers(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<ITextureMan>(() => new TextureMan(manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<ITileMan>(() => new TileMan(manCollection.GetManager<ITextureMan>(),
                                                                   manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<ITileGridFactory>(() => new TileGridFactory(manCollection.GetManager<ITileMan>(),
                                                                           manCollection.GetManager<IStampMan>(),
                                                                           manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<ISpriteMan>(() => new SpriteMan(manCollection.GetManager<ITextureMan>(),
                                                                       manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IStampMan>(() => new StampMan());

            manCollection.AddSingleton<IFontMan>(() => new FontMan(manCollection.GetManager<ITextureMan>()));

            manCollection.AddSingleton<IPrimitiveRenderer>(() => new PrimitiveRenderer());

            manCollection.AddSingleton<IRenderingMan>(() => new RenderingMan(manCollection.GetManager<IViewClient>(),
                                                                             manCollection.GetManager<IWorldMan>()));
        }

        public static void SetupSpriteSetDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register<ISpriteAtlas>(() => new SpriteAtlasDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
                                                             managerCollection.GetManager<AssetsDataProvider>(),
                                                             managerCollection.GetManager<ITextureMan>(),
                                                             managerCollection.GetManager<ISpriteMan>()));
        }

        public static void SetupTileSetDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register<ITileAtlas>(() => new TileAtlasDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
                                                             managerCollection.GetManager<AssetsDataProvider>(),
                                                             managerCollection.GetManager<ITextureMan>(),
                                                             managerCollection.GetManager<ITileMan>(),
                                                             managerCollection.GetManager<ILogger>()));
        }

        public static void SetupTileStampDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register<ITileStamp>(() => new TileStampDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
                                                             managerCollection.GetManager<AssetsDataProvider>(),
                                                             managerCollection.GetManager<ITextureMan>(),
                                                             managerCollection.GetManager<IStampMan>(),
                                                             managerCollection.GetManager<ITileMan>(),
                                                             managerCollection.GetManager<ILogger>()));
        }
    }
}

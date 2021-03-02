using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
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
            manCollection.AddSingleton<ITextureMan>(() => new TextureMan());

            manCollection.AddSingleton<ITileMan>(() => new TileMan(manCollection.GetManager<ITextureMan>()));

            manCollection.AddSingleton<ISpriteMan>(() => new SpriteMan(manCollection.GetManager<ITextureMan>(),
                                                                       manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IStampMan>(() => new StampMan());

            manCollection.AddSingleton<IFontMan>(() => new FontMan(manCollection.GetManager<ITextureMan>()));

            manCollection.AddSingleton<IPrimitiveRenderer>(() => new PrimitiveRenderer());

            manCollection.AddSingleton<IRenderingMan>(() => new RenderingMan(manCollection.GetManager<IEventsMan>(),
                                                                             manCollection.GetManager<IClientMan>(),
                                                                             manCollection.GetManager<IWorldMan>()));

        }
    }
}

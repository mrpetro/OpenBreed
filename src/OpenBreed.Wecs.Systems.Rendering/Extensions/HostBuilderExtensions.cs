using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Rendering.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupRenderingSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.RegisterSystem<ViewportSystem>(() => new ViewportSystem(
                serviceProvider.GetService<IEntityMan>(),                                        
                serviceProvider.GetService<IWorldMan>(),
                serviceProvider.GetService<IPaletteMan>(),
                serviceProvider.GetService<IPrimitiveRenderer>(),
                serviceProvider.GetService<IWindow>()));
            systemFactory.RegisterSystem<SpriteSystem>(() => new SpriteSystem(
                serviceProvider.GetService<ISpriteMan>(),
                serviceProvider.GetService<ISpriteRenderer>()));
            systemFactory.RegisterSystem<PictureSystem>(() => new PictureSystem(
                serviceProvider.GetService<IPictureRenderer>()));
            systemFactory.RegisterSystem<StampPutterSystem>(() => new StampPutterSystem());
            systemFactory.RegisterSystem<TilePutterSystem>(() => new TilePutterSystem());
            systemFactory.RegisterSystem<TileRenderSystem>(() => new TileRenderSystem());
            systemFactory.RegisterSystem<TextPresenterSystem>(() => new TextPresenterSystem(
                serviceProvider.GetService<IFontMan>()));
            systemFactory.RegisterSystem<TextSystem>(() => new TextSystem(
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IFontMan>(),
                serviceProvider.GetService<ILogger>()));
        }
    }
}

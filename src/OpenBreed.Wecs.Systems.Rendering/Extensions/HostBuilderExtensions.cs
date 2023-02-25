using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            systemFactory.RegisterSystem<ViewportSystem>((world) => new ViewportSystem(
                world,
                serviceProvider.GetService<IEntityMan>(),                                        
                serviceProvider.GetService<IWorldMan>(),
                serviceProvider.GetService<IPrimitiveRenderer>(),
                serviceProvider.GetService<IRenderingMan>(),
                serviceProvider.GetService<IViewClient>()));
            systemFactory.RegisterSystem<SpriteSystem>((world) => new SpriteSystem(
                world,
                serviceProvider.GetService<ISpriteMan>(),
                serviceProvider.GetService<ISpriteRenderer>()));
            systemFactory.RegisterSystem<PictureSystem>((world) => new PictureSystem(
                world,
                serviceProvider.GetService<IPictureRenderer>()));
            systemFactory.RegisterSystem<StampSystem>((world) => new StampSystem(world));
            systemFactory.RegisterSystem<TileSystem>((world) => new TileSystem(world));
            systemFactory.RegisterSystem<TextPresenterSystem>((world) => new TextPresenterSystem(
                world,
                serviceProvider.GetService<IFontMan>()));
            systemFactory.RegisterSystem<TextSystem>((world) => new TextSystem(
                world,
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IFontMan>(),
                serviceProvider.GetService<ILogger>()));
            systemFactory.RegisterSystem<PaletteSystem>((world) => new PaletteSystem(
                serviceProvider.GetService<IPaletteMan>(),
                serviceProvider.GetService<IPrimitiveRenderer>()));
        }
    }
}

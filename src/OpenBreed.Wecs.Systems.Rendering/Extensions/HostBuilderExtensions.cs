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
            systemFactory.Register<ViewportSystem>((world) => new ViewportSystem(
                world,
                serviceProvider.GetService<IEntityMan>(),                                        
                serviceProvider.GetService<IWorldMan>(),
                serviceProvider.GetService<IPrimitiveRenderer>(),
                serviceProvider.GetService<IRenderingMan>(),
                serviceProvider.GetService<IViewClient>()));
            systemFactory.Register<SpriteSystem>((world) => new SpriteSystem(
                world,
                serviceProvider.GetService<ISpriteMan>(),
                serviceProvider.GetService<ISpriteRenderer>()));
            systemFactory.Register<PictureSystem>((world) => new PictureSystem(
                world,
                serviceProvider.GetService<IPictureRenderer>()));
            systemFactory.Register<StampSystem>((world) => new StampSystem(world));
            systemFactory.Register<TileSystem>((world) => new TileSystem(world));
            systemFactory.Register<TextPresenterSystem>((world) => new TextPresenterSystem(
                world,
                serviceProvider.GetService<IFontMan>()));
            systemFactory.Register<TextSystem>((world) => new TextSystem(
                world,
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IFontMan>(),
                serviceProvider.GetService<ILogger>()));
        }
    }
}

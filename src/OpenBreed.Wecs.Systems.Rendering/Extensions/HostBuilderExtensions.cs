using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
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
            systemFactory.Register(() => new ViewportSystem(serviceProvider.GetService<IEntityMan>(),
                                                            serviceProvider.GetService<IWorldMan>(),
                                                            serviceProvider.GetService<IPrimitiveRenderer>(),
                                                            serviceProvider.GetService<IRenderingMan>(),
                                                            serviceProvider.GetService<IViewClient>()));
            systemFactory.Register(() => new SpriteSystem(serviceProvider.GetService<ISpriteMan>(),
                                                          serviceProvider.GetService<ISpriteRenderer>()));
            systemFactory.Register(() => new StampSystem());
            systemFactory.Register(() => new TileSystem());
            systemFactory.Register(() => new TextPresenterSystem(serviceProvider.GetService<IFontMan>()));
            systemFactory.Register(() => new TextSystem(serviceProvider.GetService<IEntityMan>(),
                                                        serviceProvider.GetService<IFontMan>(),
                                                        serviceProvider.GetService<ILogger>()));
        }
    }
}

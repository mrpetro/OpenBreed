using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Rendering.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupRenderingSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new ViewportSystem(manCollection.GetManager<IEntityMan>(),
                                                            manCollection.GetManager<IWorldMan>(),
                                                            manCollection.GetManager<IPrimitiveRenderer>(),
                                                            manCollection.GetManager<IViewClient>()));
            systemFactory.Register(() => new SpriteSystem(manCollection.GetManager<ISpriteMan>()));
            systemFactory.Register(() => new TileSystem(manCollection.GetManager<ITileMan>()));
            systemFactory.Register(() => new TextPresenterSystem(manCollection.GetManager<IFontMan>()));
            systemFactory.Register(() => new TextSystem(manCollection.GetManager<IEntityMan>(),
                                                        manCollection.GetManager<IFontMan>(),
                                                        manCollection.GetManager<ILogger>()));


            var entityCommandHandler = manCollection.GetManager<EntityCommandHandler>();

            entityCommandHandler.BindCommand<ViewportResizeCommand, ViewportSystem>();
            entityCommandHandler.BindCommand<SpriteOnCommand,SpriteSystem>();
            entityCommandHandler.BindCommand<SpriteOffCommand,SpriteSystem>();
            entityCommandHandler.BindCommand<SpriteSetCommand,SpriteSystem>();
            entityCommandHandler.BindCommand<TextSetCommand, TextSystem>();
        }
    }
}

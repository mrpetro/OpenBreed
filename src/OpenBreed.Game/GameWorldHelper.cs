using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems;
using OpenBreed.Game.Entities;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Events;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Systems.Control;

namespace OpenBreed.Game
{
    internal static class GameWorldHelper
    {
        internal static void AddSystems(Game game, WorldBuilder builder)
        {
            var systemFactory = game.GetManager<ISystemFactory>();

            //Action
            builder.AddSystem(systemFactory.Create<FollowerSystem>());

            //builder.AddSystem(game.CreateAnimationSystem().Build());

            builder.AddSystem(systemFactory.Create<TimerSystem>());
            builder.AddSystem(systemFactory.Create<FsmSystem>());

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            builder.AddSystem(systemFactory.Create<SpriteSystem>());
            builder.AddSystem(systemFactory.Create<TextSystem>());
            builder.AddSystem(systemFactory.Create<ViewportSystem>());
        }

        public static World CreateGameWorld(Game game, string worldName)
        {
            var builder = game.GetManager<IWorldMan>().Create().SetName(worldName);
            AddSystems(game, builder);

            return builder.Build(game);
        }

        internal static void Create(Game game)
        {
            World gameWorld = CreateGameWorld(game, "GameWorld");

            var cameraBuilder = new CameraBuilder(game);

            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(320 , 240);

            var playerCamera = cameraBuilder.Build();
            playerCamera.Tag = "MainCamera";

            //using (var reader = new TxtFileWorldReader(core, ".\\Content\\Maps\\hub.txt"))
            //    gameWorld = reader.GetWorld();

            var worldMan = game.GetManager<IWorldMan>();
            var eventsMan = game.GetManager<IEventsMan>();
            eventsMan.Subscribe<EntityAddedEventArgs>(worldMan,(s, a) => OnEntityAdded(game, s,a));
            eventsMan.Subscribe<EntityRemovedEventArgs>(worldMan, (s,a) => OnEntityRemoved(game,s,a));

            //var player1 = core.Players.GetByName("P1");
            //player1.AssumeControl(actor);
            //var player2 = core.Players.GetByName("P2");
            //player2.AssumeControl(actor);

            //core.Commands.Post(new AddEntityCommand(gameWorld.Id, actor.Id));
            //gameWorld.AddEntity(actor);

            gameWorld.AddEntity(playerCamera);

            var gameViewport = game.GetManager<IEntityMan>().GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            gameViewport.Get<ViewportComponent>().CameraEntityId = playerCamera.Id;
        }

        public static void SetPreserveAspectRatio(Entity viewportEntity)
        {
            var cameraEntity = viewportEntity.Core.GetManager<IEntityMan>().GetById(viewportEntity.Get<ViewportComponent>().CameraEntityId);
            viewportEntity.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(cameraEntity, a));
        }

        private static void UpdateCameraFov(Entity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.Get<CameraComponent>().Width = a.Width;
            cameraEntity.Get<CameraComponent>().Height = a.Height;
        }

        private static void OnEntityAdded(ICore core, object sender, EntityAddedEventArgs a)
        {
            var worldMan = sender as WorldMan;
            var world = worldMan.GetById(a.WorldId);
            core.Logging.Verbose($"Entity '{a.EntityId}' added to world '{world.Name}'.");
        }

        private static void OnEntityRemoved(ICore core, object sender, EntityRemovedEventArgs a)
        {
            var worldMan = sender as WorldMan;
            var world = worldMan.GetById(a.WorldId);
            core.Logging.Verbose($"Entity '{a.EntityId}' removed from world '{world.Name}'.");
        }
    }
}

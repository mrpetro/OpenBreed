using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using OpenBreed.Ecsw.Systems;
using OpenBreed.Game.Entities;
using OpenBreed.Components.Rendering;
using OpenBreed.Systems.Rendering.Events;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Systems.Core;
using OpenBreed.Ecsw;
using OpenBreed.Ecsw.Entities;
using OpenBreed.Ecsw.Worlds;

namespace OpenBreed.Game
{
    internal static class GameWorldHelper
    {
        internal static void AddSystems(Game game, WorldBuilder builder)
        {
            //Action
            builder.AddSystem(new FollowerSystem(game));

            //builder.AddSystem(game.CreateAnimationSystem().Build());

            builder.AddSystem(new TimerSystem(game));
            builder.AddSystem(new StateMachineSystem(game));

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            builder.AddSystem(game.VideoSystemsFactory.CreateSpriteSystem().Build());
            builder.AddSystem(game.VideoSystemsFactory.CreateTextSystem().Build());
            builder.AddSystem(game.VideoSystemsFactory.CreateViewportSystem().Build());
        }

        public static World CreateGameWorld(Game game, string worldName)
        {
            var builder = game.Worlds.Create().SetName(worldName);
            AddSystems(game, builder);

            return builder.Build();
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

            game.Worlds.Subscribe<EntityAddedEventArgs>(OnEntityAdded);
            game.Worlds.Subscribe<EntityRemovedEventArgs>(OnEntityRemoved);

            //var player1 = core.Players.GetByName("P1");
            //player1.AssumeControl(actor);
            //var player2 = core.Players.GetByName("P2");
            //player2.AssumeControl(actor);

            //core.Commands.Post(new AddEntityCommand(gameWorld.Id, actor.Id));
            //gameWorld.AddEntity(actor);

            gameWorld.AddEntity(playerCamera);

            var gameViewport = game.Entities.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

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

        private static void OnEntityAdded(object sender, EntityAddedEventArgs a)
        {
            var worldMan = sender as WorldMan;
            var world = worldMan.GetById(a.WorldId);
            world.Core.Logging.Verbose($"Entity '{a.EntityId}' added to world '{world.Name}'.");
        }

        private static void OnEntityRemoved(object sender, EntityRemovedEventArgs a)
        {
            var worldMan = sender as WorldMan;
            var world = worldMan.GetById(a.WorldId);
            world.Core.Logging.Verbose($"Entity '{a.EntityId}' removed from world '{world.Name}'.");
        }
    }
}

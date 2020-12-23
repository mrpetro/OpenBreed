using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using OpenBreed.Game.Entities;
using OpenBreed.Rendering.Components;
using OpenBreed.Rendering.Systems.Events;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            builder.AddSystem(game.VideoSystemsFactory.CreateSpriteSystem());
            builder.AddSystem(game.VideoSystemsFactory.CreateTextSystem());
            builder.AddSystem(game.VideoSystemsFactory.CreateViewportSystem());
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
            var cameraEntity = viewportEntity.Core.Entities.GetById(viewportEntity.Get<ViewportComponent>().CameraEntityId);
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

using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Sandbox.Components;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Systems;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Worlds
{
    public static class GameWorldHelper
    {
        public static void AddSystems(Program core, WorldBuilder builder)
        {
            int width = builder.width;
            int height = builder.height;

            //AI
            // Pathfinding/ AI systems here

            //Input
            builder.AddSystem(core.CreateWalkingControlSystem().Build());
            builder.AddSystem(core.CreateAiControlSystem().Build());

            //Action
            builder.AddSystem(core.CreateMovementSystem().Build());
            builder.AddSystem(new FollowingSystem(core));
            builder.AddSystem(core.CreatePhysicsSystem().SetGridSize(width, height).Build());
            builder.AddSystem(core.CreateAnimationSystem().Build());

            builder.AddSystem(new TimerSystem(core));
            builder.AddSystem(new StateMachineSystem(core));

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            builder.AddSystem(core.CreateTileSystem().SetGridSize(width, height)
                                                       .SetLayersNo(1)
                                                       .SetTileSize(16)
                                                       .SetGridVisible(true)
                                                       .Build());
            builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(core.CreateTextSystem().Build());

            builder.AddSystem(new UiSystem(core));

            builder.AddSystem(new ViewportSystem(core));
        }

        public static World CreateGameWorld(Program core, string worldName)
        {
            var builder = core.Worlds.Create().SetName(worldName);
            AddSystems(core, builder);

            return builder.Build();
        }

        internal static void Create(Program core)
        {
            World gameWorld = null;

            var cameraBuilder = new CameraBuilder(core);

            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(320 , 240);

            var playerCamera = cameraBuilder.Build();
            playerCamera.Tag = "PlayerCamera";
            playerCamera.Add(new AnimationComponent(10.0f, false, -1, FrameTransition.LinearInterpolation));

            cameraBuilder.SetFov(640, 480);
            var gameCamera = cameraBuilder.Build();
            gameCamera.Tag = "HubCamera";
            gameCamera.Add(new AnimationComponent(10.0f, false, -1, FrameTransition.LinearInterpolation));

            using (var reader = new TxtFileWorldReader(core, ".\\Content\\Maps\\hub.txt"))
                gameWorld = reader.GetWorld();

            //GameWorld = GameWorldHelper.CreateGameWorld(Core, "DEMO6");



            gameWorld.PostCommand(new AddEntityCommand(gameWorld.Id, playerCamera.Id));
            gameWorld.PostCommand(new AddEntityCommand(gameWorld.Id, gameCamera.Id));
            //gameWorld.AddEntity(playerCamera);
            //gameWorld.AddEntity(gameCamera);

            var actor = ActorHelper.CreateActor(core, new Vector2(128, 128));

            playerCamera.GetComponent<FollowerComponent>().FollowedEntityId = actor.Id;
            //actor.Add(new FsmComponent());
            actor.Tag = playerCamera;

            actor.Add(new WalkingControl());
            actor.Add(new AttackControl());

            //actor.Add(TextHelper.Create(core, new Vector2(0, 32), "Hero"));

            //actor.Subscribe<EntityEnteredWorldEventArgs>(OnEntityEntered);
            gameWorld.Subscribe<EntityAddedEventArgs>(OnEntityAdded);
            gameWorld.Subscribe<EntityRemovedEventArgs>(OnEntityRemoved);

            var player1 = core.Players.GetByName("P1");
            player1.AssumeControl(actor);
            var player2 = core.Players.GetByName("P2");
            player2.AssumeControl(actor);

            gameWorld.PostCommand(new AddEntityCommand(gameWorld.Id, actor.Id));
            //gameWorld.AddEntity(actor);

            var gameViewport = core.Entities.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            gameViewport.GetComponent<ViewportComponent>().CameraEntityId = playerCamera.Id;

            var cursorEntity = core.Entities.Create();
        
            var spriteBuilder = SpriteComponentBuilder.New(core);
            spriteBuilder.SetProperty("AtlasId", "Atlases/Sprites/Cursors");
            spriteBuilder.SetProperty("Order", 100.0);
            spriteBuilder.SetProperty("ImageId", 0);
            cursorEntity.Add(spriteBuilder.Build());
            cursorEntity.Add(PositionComponent.Create(0, 0));
            cursorEntity.Add(new CursorInputComponent(0));

            //gameViewport.Subscribe(GfxEventTypes.VIEWPORT_RESIZED, (s, a) => UpdateCameraFov(playerCamera, (ViewportResizedEventArgs)a));
            //SetPreserveAspectRatio(gameViewport);

            gameWorld.PostCommand(new AddEntityCommand(gameWorld.Id, cursorEntity.Id));



            //gameWorld.PostCommand(new FollowerSetTargetCommand(playerCamera.Id, actor.Id));
        }

        public static void SetPreserveAspectRatio(IEntity viewportEntity)
        {
            var cameraEntity = viewportEntity.Core.Entities.GetById(viewportEntity.GetComponent<ViewportComponent>().CameraEntityId);
            viewportEntity.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(cameraEntity, a));
        }

        private static void UpdateCameraFov(IEntity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.GetComponent<CameraComponent>().Width = a.Width;
            cameraEntity.GetComponent<CameraComponent>().Height = a.Height;
        }

        private static void OnEntityAdded(object sender, EntityAddedEventArgs a)
        {
            var world = sender as World;
            world.Core.Logging.Verbose($"Entity '{a.EntityId}' added to world '{world.Name}'.");
        }

        private static void OnEntityRemoved(object sender, EntityRemovedEventArgs a)
        {
            var world = sender as World;
            world.Core.Logging.Verbose($"Entity '{a.EntityId}' removed from world '{world.Name}'.");
        }
    }
}

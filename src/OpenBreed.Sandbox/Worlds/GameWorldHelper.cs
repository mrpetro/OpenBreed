﻿using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Builders;
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
            builder.AddSystem(new DirectionSystem(core));
            builder.AddSystem(new FollowerSystem(core));
            //builder.AddSystem(new FollowerSystem(core));
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

            var animCmpBuilder = AnimationComponentBuilderEx.New(core);
            animCmpBuilder.SetSpeed(10.0f);
            animCmpBuilder.SetLoop(false);
            animCmpBuilder.SetById(-1);
            animCmpBuilder.SetTransition(FrameTransition.LinearInterpolation);

            playerCamera.Add(animCmpBuilder.Build());

            cameraBuilder.SetFov(640, 480);
            var gameCamera = cameraBuilder.Build();
            gameCamera.Tag = "HubCamera";

            animCmpBuilder = AnimationComponentBuilderEx.New(core);
            animCmpBuilder.SetSpeed(10.0f);
            animCmpBuilder.SetLoop(false);
            animCmpBuilder.SetById(-1);
            animCmpBuilder.SetTransition(FrameTransition.LinearInterpolation);

            gameCamera.Add(animCmpBuilder.Build());

            using (var reader = new TxtFileWorldReader(core, ".\\Content\\Maps\\hub.txt"))
                gameWorld = reader.GetWorld();

            var actor = ActorHelper.CreateActor(core, new Vector2(128, 128));

            actor.Add(new WalkingControl());
            actor.Add(new AttackControl());

            //actor.Add(TextHelper.Create(core, new Vector2(0, 32), "Hero"));

            //actor.Subscribe<EntityEnteredWorldEventArgs>(OnEntityEntered);
            core.Worlds.Subscribe<EntityAddedEventArgs>(OnEntityAdded);
            core.Worlds.Subscribe<EntityRemovedEventArgs>(OnEntityRemoved);

            var player1 = core.Players.GetByName("P1");
            player1.AssumeControl(actor);
            var player2 = core.Players.GetByName("P2");
            player2.AssumeControl(actor);

            core.Commands.Post(new AddEntityCommand(gameWorld.Id, actor.Id));
            //gameWorld.AddEntity(actor);

            var gameViewport = core.Entities.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            gameViewport.Get<ViewportComponent>().CameraEntityId = playerCamera.Id;

            var cursorEntity = core.Entities.Create();
        
            var spriteBuilder = SpriteComponentBuilderEx.New(core);
            spriteBuilder.SetAtlasByName("Atlases/Sprites/Cursors");
            spriteBuilder.SetOrder(100);
            spriteBuilder.SetImageId(0);
            cursorEntity.Tag = "MouseCursor";
            cursorEntity.Add(spriteBuilder.Build());
            cursorEntity.Add(PositionComponent.Create(0, 0));
            cursorEntity.Add(new CursorInputComponent(0));

            //gameViewport.Subscribe(GfxEventTypes.VIEWPORT_RESIZED, (s, a) => UpdateCameraFov(playerCamera, (ViewportResizedEventArgs)a));
            //SetPreserveAspectRatio(gameViewport);

            core.Commands.Post(new AddEntityCommand(gameWorld.Id, cursorEntity.Id));

            core.Commands.Post(new FollowedAddFollowerCommand(actor.Id, playerCamera.Id));
            //gameWorld.PostCommand(new FollowerSetTargetCommand(playerCamera.Id, actor.Id));
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

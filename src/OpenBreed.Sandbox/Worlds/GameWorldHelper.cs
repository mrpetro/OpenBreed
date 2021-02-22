﻿using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Sandbox.Components;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Systems;
using OpenBreed.Wecs.Systems.Physics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Systems.Control.Commands;
using OpenBreed.Wecs.Systems.Control.Systems;
using OpenBreed.Wecs.Events;
using OpenBreed.Input.Interface;
using OpenBreed.Fsm;

namespace OpenBreed.Sandbox.Worlds
{
    public static class GameWorldHelper
    {
        public static void AddSystems(Program core, WorldBuilder builder)
        {
            var systemFactory = core.GetManager<ISystemFactory>();
            var renderingModule = core.GetModule<IRenderModule>();

            int width = builder.width;
            int height = builder.height;

            //AI
            // Pathfinding/ AI systems here

            //Input
            builder.AddSystem(core.CreateWalkingControlSystem().Build());
            builder.AddSystem(core.CreateAiControlSystem().Build());
            builder.AddSystem(new WalkingControllerSystem(core));
            builder.AddSystem(new AttackControllerSystem(core));

            //Action
            builder.AddSystem(core.CreateMovementSystem().Build());
            builder.AddSystem(new DirectionSystem(core, core.GetManager<IEntityMan>()));
            builder.AddSystem(new FollowerSystem(core, core.GetManager<IEntityMan>()));
            //builder.AddSystem(new FollowerSystem(core));
            builder.AddSystem(core.CreatePhysicsSystem().SetGridSize(width, height).Build());
            builder.AddSystem(core.CreateAnimationSystem().Build());

            builder.AddSystem(new TimerSystem(core, core.GetManager<IEntityMan>()));
            builder.AddSystem(new FsmSystem(core, core.GetManager<IFsmMan>()));

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            builder.AddSystem(core.VideoSystemsFactory.CreateTileSystem().SetGridSize(width, height)
                                                       .SetLayersNo(1)
                                                       .SetTileSize(16)
                                                       .SetGridVisible(true)
                                                       .Build());
            builder.AddSystem(core.VideoSystemsFactory.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(systemFactory.Create<TextSystem>());

            builder.AddSystem(new UiSystem(core, renderingModule, core.GetManager<IInputsMan>()));

            builder.AddSystem(core.VideoSystemsFactory.CreateViewportSystem().Build());
        }

        public static World CreateGameWorld(Program core, string worldName)
        {
            var builder = core.GetManager<IWorldMan>().Create().SetName(worldName);
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

            var animCmpBuilder = AnimationComponentBuilder.New(core);
            animCmpBuilder.SetSpeed(10.0f);
            animCmpBuilder.SetLoop(false);
            animCmpBuilder.SetById(-1);
            animCmpBuilder.SetTransition(FrameTransition.LinearInterpolation);

            playerCamera.Add(animCmpBuilder.Build());

            cameraBuilder.SetFov(640, 480);
            var gameCamera = cameraBuilder.Build();
            gameCamera.Tag = "HubCamera";

            animCmpBuilder = AnimationComponentBuilder.New(core);
            animCmpBuilder.SetSpeed(10.0f);
            animCmpBuilder.SetLoop(false);
            animCmpBuilder.SetById(-1);
            animCmpBuilder.SetTransition(FrameTransition.LinearInterpolation);

            gameCamera.Add(animCmpBuilder.Build());

            using (var reader = new TxtFileWorldReader(core, ".\\Content\\Maps\\hub.txt"))
                gameWorld = reader.GetWorld();

            var actor = ActorHelper.CreateActor(core, new Vector2(128, 128));

            var p1 = core.Players.GetByName("P1");

            actor.Add(new WalkingInputComponent(p1.Id, 0));
            actor.Add(new AttackInputComponent(p1.Id, 0));
            actor.Add(new WalkingControlComponent());
            actor.Add(new AttackControlComponent());

            //actor.Add(TextHelper.Create(core, new Vector2(0, 32), "Hero"));

            //actor.Subscribe<EntityEnteredWorldEventArgs>(OnEntityEntered);
            core.GetManager<IWorldMan>().Subscribe<EntityAddedEventArgs>(OnEntityAdded);
            core.GetManager<IWorldMan>().Subscribe<EntityRemovedEventArgs>(OnEntityRemoved);

            core.Commands.Post(new AddEntityCommand(gameWorld.Id, actor.Id));
            //gameWorld.AddEntity(actor);

            var gameViewport = core.GetManager<IEntityMan>().GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            gameViewport.Get<ViewportComponent>().CameraEntityId = playerCamera.Id;

            var cursorEntity = core.GetManager<IEntityMan>().Create();
        
            var spriteBuilder = SpriteComponentBuilder.New(core);
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

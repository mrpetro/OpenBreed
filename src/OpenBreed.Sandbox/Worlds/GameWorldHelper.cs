﻿using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Events;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Helpers;
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
            builder.AddSystem(core.CreatePhysicsSystem().SetGridSize(width, height).Build());
            builder.AddSystem(core.CreateAnimationSystem().Build());

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

            builder.AddSystem(new ViewportSystem(core));
        }

        public static World CreateGameWorld(Program core, string worldName)
        {
            var builder = core.Worlds.Create().SetName(worldName);
            AddSystems(core, builder);

            return builder.Build();
        }

        internal static void CreateGameWorld(Program core)
        {
            World gameWorld = null;

            var cameraBuilder = new CameraBuilder(core);

            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(320 , 240);

            var playerCamera = cameraBuilder.Build();
            playerCamera.Tag = "PlayerCamera";
            playerCamera.Add(new Animator(10.0f, false, -1, FrameTransition.LinearInterpolation));

            cameraBuilder.SetZoom(640, 480);
            var gameCamera = cameraBuilder.Build();
            gameCamera.Tag = "HubCamera";
            gameCamera.Add(new Animator(10.0f, false, -1, FrameTransition.LinearInterpolation));

            using (var reader = new TxtFileWorldReader(core, ".\\Content\\Maps\\hub.txt"))
                gameWorld = reader.GetWorld();

            //GameWorld = GameWorldHelper.CreateGameWorld(Core, "DEMO6");




            gameWorld.AddEntity(playerCamera);
            gameWorld.AddEntity(gameCamera);

            var actor = ActorHelper.CreateActor(core, new Vector2(128, 128));
            actor.Tag = playerCamera;

            actor.Add(new WalkingControl());
            actor.Add(new AttackControl());

            actor.Add(TextHelper.Create(core, new Vector2(0, 32), "Hero"));

            actor.Subscribe(CoreEventTypes.ENTITY_ENTERED_WORLD, OnEntityEntered);
            actor.Subscribe(CoreEventTypes.ENTITY_LEFT_WORLD, OnEntityLeftWorld);


            core.Jobs.Execute(new CameraFollowJob(playerCamera, actor));

            var player1 = core.Players.GetByName("P1");
            player1.AssumeControl(actor);
            var player2 = core.Players.GetByName("P2");
            player2.AssumeControl(actor);

            var movementFsm = ActorHelper.CreateMovementFSM(actor);
            var atackFsm = ActorHelper.CreateAttackingFSM(actor);
            var rotateFsm = ActorHelper.CreateRotationFSM(actor);
            movementFsm.SetInitialState("Standing");
            atackFsm.SetInitialState("Idle");
            rotateFsm.SetInitialState("Idle");
            gameWorld.AddEntity(actor);

            var gameViewport = core.Entities.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            gameViewport.GetComponent<ViewportComponent>().CameraEntityId = playerCamera.Id;

            //gameViewport.Subscribe(GfxEventTypes.VIEWPORT_RESIZED, (s, a) => UpdateCameraFov(playerCamera, (ViewportResizedEventArgs)a));
            //SetPreserveAspectRatio(gameViewport);
        }

        public static void SetPreserveAspectRatio(IEntity viewportEntity)
        {
            var cameraEntity = viewportEntity.Core.Entities.GetById(viewportEntity.GetComponent<ViewportComponent>().CameraEntityId);
            viewportEntity.Subscribe(GfxEventTypes.VIEWPORT_RESIZED, (s, a) => UpdateCameraFov(cameraEntity, (ViewportResizedEventArgs)a));
        }

        private static void UpdateCameraFov(IEntity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.GetComponent<CameraComponent>().Width = a.Width;
            cameraEntity.GetComponent<CameraComponent>().Height = a.Height;
        }

        private static void OnEntityEntered(object sender, EventArgs e)
        {
            var ea = (EntityEnteredWorldEventArgs)e;
            ea.Entity.Core.Logging.Verbose($"Entity '{ea.Entity.Id}' entered world '{ea.World.Name}'.");
        }

        private static void OnEntityLeftWorld(object sender, EventArgs e)
        {
            var ea = (EntityLeftWorldEventArgs)e;
            ea.Entity.Core.Logging.Verbose($"Entity '{ea.Entity.Id}' left world '{ea.World.Name}'.");

        }
    }
}

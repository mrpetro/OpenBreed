using OpenBreed.Core;
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
using OpenBreed.Wecs.Events;
using OpenBreed.Input.Interface;
using OpenBreed.Fsm;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Gui;
using OpenBreed.Wecs.Components.Gui;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Common;

namespace OpenBreed.Sandbox.Worlds
{
    public class GameWorldHelper
    {
        private readonly ICore core;
        private readonly IManagerCollection managerCollection;
        private readonly IPlayersMan playersMan;
        private readonly ICommandsMan commandsMan;
        private readonly IEntityMan entityMan;
        private readonly ISystemFactory systemFactory;
        private readonly IWorldMan worldMan;
        private readonly IRenderingMan renderingMan;
        private readonly IEventsMan eventsMan;

        public GameWorldHelper(ICore core, IManagerCollection managerCollection, IPlayersMan playersMan, ICommandsMan commandsMan, IEntityMan entityMan, ISystemFactory systemFactory, IWorldMan worldMan, IRenderingMan renderingMan, IEventsMan eventsMan)
        {
            this.core = core;
            this.managerCollection = managerCollection;
            this.playersMan = playersMan;
            this.commandsMan = commandsMan;
            this.entityMan = entityMan;
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            this.renderingMan = renderingMan;
            this.eventsMan = eventsMan;
        }

        public void AddSystems(WorldBuilder builder)
        {
            int width = builder.width;
            int height = builder.height;

            //AI
            // Pathfinding/ AI systems here

            //Input
            builder.AddSystem(systemFactory.Create<WalkingControlSystem>());
            builder.AddSystem(systemFactory.Create<AiControlSystem>());
            builder.AddSystem(systemFactory.Create<WalkingControllerSystem>());
            builder.AddSystem(systemFactory.Create<AttackControllerSystem>());

            //Action
            builder.AddSystem(systemFactory.Create<MovementSystem>());
            builder.AddSystem(systemFactory.Create<DirectionSystem>());
            builder.AddSystem(systemFactory.Create<FollowerSystem>());
            //builder.AddSystem(new FollowerSystem(core));
            builder.AddSystem(systemFactory.Create<PhysicsSystem>());
            builder.AddSystem(systemFactory.Create<AnimationSystem>());
            builder.AddSystem(systemFactory.Create<TimerSystem>());
            builder.AddSystem(systemFactory.Create<FsmSystem>());

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            builder.AddSystem(systemFactory.Create<TileSystem>());
            builder.AddSystem(systemFactory.Create<SpriteSystem>());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(systemFactory.Create<TextSystem>());

            builder.AddSystem(systemFactory.Create<UiSystem>());

            builder.AddSystem(systemFactory.Create<ViewportSystem>());
        }

        public World CreateGameWorld(string worldName)
        {
            var builder = worldMan.Create().SetName(worldName);
            AddSystems(builder);

            return builder.Build(core);
        }

        internal void Create()
        {
            World gameWorld = null;

            var cameraBuilder = new CameraBuilder(core);

            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(320 , 240);

            var playerCamera = cameraBuilder.Build();
            playerCamera.Tag = "PlayerCamera";

            var animCmpBuilder = core.GetManager<AnimationComponentBuilder>();
            animCmpBuilder.SetSpeed(10.0f);
            animCmpBuilder.SetLoop(false);
            animCmpBuilder.SetById(-1);
            animCmpBuilder.SetTransition(FrameTransition.LinearInterpolation);

            playerCamera.Add(animCmpBuilder.Build());

            cameraBuilder.SetFov(640, 480);
            var gameCamera = cameraBuilder.Build();
            gameCamera.Tag = "HubCamera";

            animCmpBuilder = managerCollection.GetManager<AnimationComponentBuilder>();
            animCmpBuilder.SetSpeed(10.0f);
            animCmpBuilder.SetLoop(false);
            animCmpBuilder.SetById(-1);
            animCmpBuilder.SetTransition(FrameTransition.LinearInterpolation);

            gameCamera.Add(animCmpBuilder.Build());

            using (var reader = new TxtFileWorldReader(core, ".\\Content\\Maps\\hub.txt"))
                gameWorld = reader.GetWorld();

            var actor = ActorHelper.CreateActor(core, new Vector2(128, 128));

            var p1 = playersMan.GetByName("P1");

            actor.Add(new WalkingInputComponent(p1.Id, 0));
            actor.Add(new AttackInputComponent(p1.Id, 0));
            actor.Add(new WalkingControlComponent());
            actor.Add(new AttackControlComponent());

            //actor.Add(TextHelper.Create(core, new Vector2(0, 32), "Hero"));

            //actor.Subscribe<EntityEnteredWorldEventArgs>(OnEntityEntered);
            eventsMan.Subscribe<EntityAddedEventArgs>(worldMan, (s,a) => OnEntityAdded(s,a));
            eventsMan.Subscribe<EntityRemovedEventArgs>(worldMan, (s,a) => OnEntityRemoved(s,a));

            core.Commands.Post(new AddEntityCommand(gameWorld.Id, actor.Id));
            //gameWorld.AddEntity(actor);

            var gameViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            gameViewport.Get<ViewportComponent>().CameraEntityId = playerCamera.Id;

            var cursorEntity = entityMan.Create(core);
        
            var spriteBuilder = managerCollection.GetManager<SpriteComponentBuilder>();
            spriteBuilder.SetAtlasByName("Atlases/Sprites/Cursors");
            spriteBuilder.SetOrder(100);
            spriteBuilder.SetImageId(0);
            cursorEntity.Tag = "MouseCursor";
            cursorEntity.Add(spriteBuilder.Build());
            cursorEntity.Add(PositionComponent.Create(0, 0));
            cursorEntity.Add(new CursorInputComponent(0));

            //gameViewport.Subscribe(GfxEventTypes.VIEWPORT_RESIZED, (s, a) => UpdateCameraFov(playerCamera, (ViewportResizedEventArgs)a));
            //SetPreserveAspectRatio(gameViewport);

            commandsMan.Post(new AddEntityCommand(gameWorld.Id, cursorEntity.Id));

            commandsMan.Post(new FollowedAddFollowerCommand(actor.Id, playerCamera.Id));
            //gameWorld.PostCommand(new FollowerSetTargetCommand(playerCamera.Id, actor.Id));
        }

        public void SetPreserveAspectRatio(Entity viewportEntity)
        {
            var cameraEntity = entityMan.GetById(viewportEntity.Get<ViewportComponent>().CameraEntityId);
            viewportEntity.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(cameraEntity, a));
        }

        private void UpdateCameraFov(Entity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.Get<CameraComponent>().Width = a.Width;
            cameraEntity.Get<CameraComponent>().Height = a.Height;
        }

        private void OnEntityAdded(object sender, EntityAddedEventArgs a)
        {
            var world = worldMan.GetById(a.WorldId);
            core.Logging.Verbose($"Entity '{a.EntityId}' added to world '{world.Name}'.");
        }

        private void OnEntityRemoved(object sender, EntityRemovedEventArgs a)
        {
            var world = worldMan.GetById(a.WorldId);
            core.Logging.Verbose($"Entity '{a.EntityId}' removed from world '{world.Name}'.");
        }
    }
}

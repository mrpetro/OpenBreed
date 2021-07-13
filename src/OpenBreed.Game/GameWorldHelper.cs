using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Game.Entities;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Linq;

namespace OpenBreed.Game
{
    internal class GameWorldHelper
    {
        #region Private Fields

        private readonly ICore core;
        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly ISystemFactory systemFactory;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public GameWorldHelper(ICore core, IWorldMan worldMan, IEntityMan entityMan, ISystemFactory systemFactory, IEventsMan eventsMan, ILogger logger)
        {
            this.core = core;
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.systemFactory = systemFactory;
            this.eventsMan = eventsMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public World CreateGameWorld(string worldName)
        {
            var builder = worldMan.Create().SetName(worldName);
            AddSystems(builder);

            return builder.Build(core);
        }

        public void SetPreserveAspectRatio(IEventsMan eventsMan, Entity viewportEntity)
        {
            var cameraEntity = entityMan.GetById(viewportEntity.Get<ViewportComponent>().CameraEntityId);

            eventsMan.Subscribe<ViewportResizedEventArgs>(viewportEntity, (s, a) => UpdateCameraFov(cameraEntity, a));
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddSystems(WorldBuilder builder)
        {
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

        internal void Create()
        {
            World gameWorld = CreateGameWorld("GameWorld");

            var cameraBuilder = core.GetManager<CameraBuilder>();

            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(320, 240);

            var playerCamera = cameraBuilder.Build();
            playerCamera.Tag = "MainCamera";

            //using (var reader = new TxtFileWorldReader(core, ".\\Content\\Maps\\hub.txt"))
            //    gameWorld = reader.GetWorld();

            eventsMan.Subscribe<EntityAddedEventArgs>(worldMan, (s, a) => OnEntityAdded(s, a));
            eventsMan.Subscribe<EntityRemovedEventArgs>(worldMan, (s, a) => OnEntityRemoved(s, a));

            //var player1 = core.Players.GetByName("P1");
            //player1.AssumeControl(actor);
            //var player2 = core.Players.GetByName("P2");
            //player2.AssumeControl(actor);

            //core.Commands.Post(new AddEntityCommand(gameWorld.Id, actor.Id));
            //gameWorld.AddEntity(actor);

            gameWorld.AddEntity(playerCamera);

            var gameViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            gameViewport.Get<ViewportComponent>().CameraEntityId = playerCamera.Id;
        }

        #endregion Internal Methods

        #region Private Methods

        private void UpdateCameraFov(Entity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.Get<CameraComponent>().Width = a.Width;
            cameraEntity.Get<CameraComponent>().Height = a.Height;
        }

        private void OnEntityAdded(object sender, EntityAddedEventArgs a)
        {
            var world = worldMan.GetById(a.WorldId);
            logger.Verbose($"Entity '{a.EntityId}' added to world '{world.Name}'.");
        }

        private void OnEntityRemoved(object sender, EntityRemovedEventArgs a)
        {
            var world = worldMan.GetById(a.WorldId);
            logger.Verbose($"Entity '{a.EntityId}' removed from world '{world.Name}'.");
        }

        #endregion Private Methods
    }
}
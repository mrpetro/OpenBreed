using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Physics.Abstractions;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Systems
{
    public class Actor2ExitCollisionHandlerSystem : IOnEntityCollisionSystem
    {
        #region Private Fields

        private readonly ILogger logger;

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly ITriggerMan triggerMan;
        private readonly IScriptMan scriptMan;
        private readonly IClipMan<IEntity> clipMan;
        private readonly IDataLoaderFactory dataLoaderFactory;

        #endregion Private Fields

        #region Public Constructors

        public Actor2ExitCollisionHandlerSystem(
            ILogger logger,
            IEntityMan entityMan,
            IWorldMan worldMan,
            ITriggerMan triggerMan,
            IScriptMan scriptMan,
            IClipMan<IEntity> clipMan,
            IDataLoaderFactory dataLoaderFactory)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.entityMan = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            this.worldMan = worldMan ?? throw new ArgumentNullException(nameof(worldMan));
            this.triggerMan = triggerMan ?? throw new ArgumentNullException(nameof(triggerMan));
            this.scriptMan = scriptMan ?? throw new ArgumentNullException(nameof(scriptMan));
            this.clipMan = clipMan ?? throw new ArgumentNullException(nameof(clipMan));
            this.dataLoaderFactory = dataLoaderFactory ?? throw new ArgumentNullException(nameof(dataLoaderFactory));
        }

        #endregion Public Constructors

        #region Public Properties

        public int ColliderTypeA => ColliderTypes.ActorBody;

        public IEnumerable<int> ColliderTypesB
        {
            get
            {
                yield return ColliderTypes.WorldExitTrigger;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnCollision(IFixture FixtureA, IEntity actorEntity, IFixture fixtureB, IEntity exitEntity, float dt, Vector2 projection)
        {
            // For preventing running rest of the code when actor will hit couple of teleporter blocks at same time
            if (Equals(actorEntity.State, "Exiting"))
                return;

            actorEntity.State = "Exiting";

            var cameraEntity = actorEntity.TryGet<FollowedComponent>()?.FollowerIds.
                                                                              Select(item => entityMan.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "Camera.Player");

            if (cameraEntity == null)
                return;

            var matadataCmp = exitEntity.Get<MetadataComponent>();

            if (!int.TryParse(matadataCmp.Flavor, out int exitId))
                throw new InvalidOperationException("Expected exit number");

            var mapId = exitId % 64;
            var entryId = exitId / 64;

            var mapKey = $"Vanilla/{mapId}";

            var worldIdToRemoveFrom = actorEntity.WorldId;

            var actorWorld = worldMan.GetById(actorEntity.WorldId);
            var cameraWorld = worldMan.GetById(cameraEntity.WorldId);
            //var doorOpening = PerformFunction(() => door.TryOpen(key));
            //var doorClosing = doorOpening.OnFinishResult((result) => result == "Matching").PerformAction(() => door.Close())
            //door.Wait(5).OnFinish((door) => door.Close())
            //door.Close()

            var context = new TransferContext()
            {
                actorEntity = actorEntity,
                cameraEntity = cameraEntity,
                mapKey = mapKey,
                entryId = entryId
            };

            PauseWorld(context)
                .Then(FadeOut)
                .Then(RemoveFromWorld)
                .Then(LoadWorld)
                .Then(AddToWorld)
                .Then(PlayerCharacterEnter);
        }

        #endregion Public Methods

        #region Private Methods

        private TransferContext PauseWorld(TransferContext context)
        {
            triggerMan.OnPausedWorld(context.cameraEntity, (e, a) =>
            {
                context.InvokeNextJob();
            }, singleTime: true);

            context.cameraEntity.PauseWorld();

            return context;
        }

        private TransferContext FadeOut(TransferContext context)
        {
            var cameraFadeOutClipId = clipMan.GetId(CameraHelper.CAMERA_FADE_OUT);

            triggerMan.OnEntityAnimFinished(context.cameraEntity, (e, a) =>
            {
                context.InvokeNextJob();
            }, singleTime: true);

            context.cameraEntity.PlayAnimation(0, cameraFadeOutClipId);

            return context;
        }

        private TransferContext LoadWorld(TransferContext context)
        {
            context.targetWorld = TryLoadWorld(context.mapKey);

            triggerMan.OnWorldInitialized(context.targetWorld, () =>
            {
                context.InvokeNextJob();
            }, singleTime: true);

            return context;
        }

        private TransferContext RemoveFromWorld(TransferContext context)
        {
            triggerMan.OnEntityLeftWorld(context.actorEntity, (s, a) =>
            {
                context.InvokeNextJob();
            }, singleTime: true);

            worldMan.RequestRemoveEntity(context.actorEntity);

            return context;
        }

        private TransferContext AddToWorld(TransferContext context)
        {
            triggerMan.OnEntityEnteredWorld(context.cameraEntity, (e, args) =>
            {
                context.InvokeNextJob();
            }, singleTime: true);

            AddToWorld(context.actorEntity, context.mapKey);

            return context;
        }

        private TransferContext PlayerCharacterEnter(TransferContext context)
        {
            context.actorEntity.TryInvoke(scriptMan, logger, "OnEnter");

            worldMan.SetEntityPosition(context.actorEntity, context.entryId);

            return context;
        }

        private void AddToWorld(IEntity target, string worldName)
        {
            var world = worldMan.GetByName(worldName);

            worldMan.RequestAddEntity(target, world.Id);
        }

        private IWorld TryLoadWorld(string worldName)
        {
            var world = worldMan.GetByName(worldName);

            if (world is null)
            {
                var mapWorldDataLoader = dataLoaderFactory.GetLoader<MapLegacyDataLoader>();
                world = mapWorldDataLoader.Load(worldName);
            }

            return world;
        }

        #endregion Private Methods
    }
}
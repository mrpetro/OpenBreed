using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Entities;
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

            var cameraFadeOutClipId = clipMan.GetId(CameraHelper.CAMERA_FADE_OUT);
            var cameraFadeInClipId = clipMan.GetId(CameraHelper.CAMERA_FADE_IN);

            var worldIdToRemoveFrom = actorEntity.WorldId;

            var actorWorld = worldMan.GetById(actorEntity.WorldId);
            var cameraWorld = worldMan.GetById(cameraEntity.WorldId);
            //var doorOpening = PerformFunction(() => door.TryOpen(key));
            //var doorClosing = doorOpening.OnFinishResult((result) => result == "Matching").PerformAction(() => door.Close())
            //door.Wait(5).OnFinish((door) => door.Close())
            //door.Close()

            var context = new Context()
            {
                actorEntity = actorEntity,
                cameraEntity = cameraEntity,
                cameraFadeInClipId = cameraFadeInClipId,
                cameraFadeOutClipId = cameraFadeOutClipId,
                mapKey = mapKey,
                entryId = entryId
            };

            PauseWorld(context);
            //    .Then(FadeOut)
            //    .Then(RemoveFromWorld)
            //    .Then(RemoveFromWorld);
        }

        #endregion Public Methods

        #region Private Methods

        private Context PauseWorld(Context context)
        {
            triggerMan.OnPausedWorld(context.cameraEntity, (e, a) =>
            {
                FadeOut(context);
            }, singleTime: true);

            context.cameraEntity.PauseWorld();

            return context;
        }

        private Context FadeOut(Context context)
        {
            triggerMan.OnEntityAnimFinished(context.cameraEntity, (e, a) =>
            {
                RemoveFromWorld(context);
            }, singleTime: true);

            context.cameraEntity.PlayAnimation(0, context.cameraFadeOutClipId);

            return context;
        }

        private void LoadWorld(Context context)
        {
            context.targetWorld = TryLoadWorld(context.mapKey);

            triggerMan.OnWorldInitialized(context.targetWorld, () =>
            {
                AddToWorld(context);
            }, singleTime: true);
        }

        private Context RemoveFromWorld(Context context)
        {
            triggerMan.OnEntityLeftWorld(context.actorEntity, (s, a) =>
            {
                LoadWorld(context);
            }, singleTime: true);

            worldMan.RequestRemoveEntity(context.actorEntity);

            return context;
        }

        private void AddToWorld(Context context)
        {
            triggerMan.OnEntityEnteredWorld(context.cameraEntity, (e, args) =>
            {
                PlayerCharacterEnter(context);
                SetPosition(context);
                //FadeIn(context);
            }, singleTime: true);

            AddToWorld(context.actorEntity, context.mapKey);
        }

        private void PlayerCharacterEnter(Context context)
        {
            context.actorEntity.TryInvoke(scriptMan, logger, "OnEnter");
        }

        private void SetPosition(Context context)
        {
            SetPosition(context.actorEntity, context.entryId);
        }

        private void Actor2TriggerCallbackEx(IFixture fixtureA, IEntity entityA, IFixture fixtureB, IEntity entityB, float dt, Vector2 projection)
        {
            PerformEntityExit(entityA, entityB);
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

        private void PerformEntityExit(IEntity actorEntity, IEntity exitEntity)
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

            var cameraFadeOutClipId = clipMan.GetId(CameraHelper.CAMERA_FADE_OUT);
            var cameraFadeInClipId = clipMan.GetId(CameraHelper.CAMERA_FADE_IN);

            var worldIdToRemoveFrom = actorEntity.WorldId;

            var actorWorld = worldMan.GetById(actorEntity.WorldId);
            var cameraWorld = worldMan.GetById(cameraEntity.WorldId);
            //var doorOpening = PerformFunction(() => door.TryOpen(key));
            //var doorClosing = doorOpening.OnFinishResult((result) => result == "Matching").PerformAction(() => door.Close())
            //door.Wait(5).OnFinish((door) => door.Close())
            //door.Close()

            var context = new Context()
            {
                actorEntity = actorEntity,
                cameraEntity = cameraEntity,
                cameraFadeInClipId = cameraFadeInClipId,
                cameraFadeOutClipId = cameraFadeOutClipId,
                mapKey = mapKey,
                entryId = entryId
            };

            PauseWorld(context);
            //    .Then(FadeOut)
            //    .Then(RemoveFromWorld)
            //    .Then(RemoveFromWorld);
        }

        private IEnumerable<IEntity> FindEntryEntities(IWorld world, int entryId)
        {
            foreach (var entity in world.Entities.Where(e => e.Contains<MetadataComponent>()))
            {
                var cmpClass = entity.Get<MetadataComponent>();

                if (cmpClass.Name != "WorldEntry")
                    continue;

                if (cmpClass.Flavor != entryId.ToString())
                    continue;

                yield return entity;
            }
        }

        /// <summary>
        /// This function should emulate scanline method from vanilla ABTA for searching
        /// Entities
        /// </summary>
        /// <param name="entities">Entities to check coordinates</param>
        /// <returns></returns>
        private IEntity GetTopLeftMostEntity(IEnumerable<IEntity> entities)
        {
            IEntity topMostEntity = null;
            var topMostPosX = float.MaxValue;
            var topMostPosY = 0.0f;

            foreach (var entity in entities)
            {
                var pos = entity.Get<PositionComponent>().Value;

                if (pos.Y < topMostPosY)
                    continue;

                if (pos.Y == topMostPosY)
                {
                    if (pos.X > topMostPosX)
                        continue;
                }

                topMostPosX = pos.X;
                topMostPosY = pos.Y;
                topMostEntity = entity;
            }

            return topMostEntity;
        }

        private void SetPosition(IEntity target, int entryId)
        {
            var world = worldMan.GetById(target.WorldId);

            var entryEntity = GetTopLeftMostEntity(FindEntryEntities(world, entryId));

            if (entryEntity is null)
                entryEntity = GetTopLeftMostEntity(FindEntryEntities(world, 2));

            if (entryEntity is null)
                throw new Exception($"No entry with ID '{entryId}' found.");

            var entryPos = entryEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();
            //var targetAabb = broadphase.GetAabb(target.Id);
            //var offset = new Vector2((32 - targetAabb.Width) / 2.0f, (32 - targetAabb.Height) / 2.0f);

            var newPosition = entryPos.Value;// + offset;

            targetPos.Value = newPosition;

            var velocityCmp = target.Get<VelocityComponent>();
            velocityCmp.Value = Vector2.Zero;

            var thrustCmp = target.Get<ThrustComponent>();
            thrustCmp.Value = Vector2.Zero;

            target.State = null;
        }

        #endregion Private Methods

        #region Private Classes

        private class Context
        {
            #region Public Properties

            public IEntity cameraEntity { get; set; }
            public IEntity actorEntity { get; set; }
            public int cameraFadeOutClipId { get; set; }
            public int cameraFadeInClipId { get; set; }
            public string mapKey { get; set; }
            public int entryId { get; set; }
            public IWorld targetWorld { get; internal set; }

            #endregion Public Properties

            #region Public Methods

            public Context Then(Func<Context, Context> function)
            {
                return function.Invoke(this);
            }

            #endregion Public Methods
        }

        #endregion Private Classes
    }
}
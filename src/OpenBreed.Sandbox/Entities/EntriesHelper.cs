using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Scripting;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using static OpenBreed.Wecs.Components.Animation.AnimationPlayerComponent;

namespace OpenBreed.Sandbox.Entities
{
    public class EntriesHelper
    {
        #region Public Fields

        public const string SPRITE_WORLD_ENTRY = "Atlases/Sprites/World/Entry";

        public const string SPRITE_WORLD_EXIT = "Atlases/Sprites/World/Exit";

        #endregion Public Fields

        #region Private Fields

        private readonly IWorldMan worldMan;

        private readonly IEntityMan entityMan;
        private readonly ITriggerMan triggerMan;
        private readonly IClipMan<IEntity> clipMan;

        private readonly IEntityFactory entityFactory;

        private readonly IEventsMan eventsMan;

        private readonly ICollisionMan<IEntity> collisionMan;

        private readonly IJobsMan jobsMan;

        private readonly ViewportCreator viewportCreator;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IScriptMan scriptMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public EntriesHelper(
            IWorldMan worldMan,
            IEntityMan entityMan,
            ITriggerMan triggerMan,
            IClipMan<IEntity> clipMan,
            IEntityFactory entityFactory,
            IEventsMan eventsMan,
            ICollisionMan<IEntity> collisionMan,
            IJobsMan jobsMan,
            ViewportCreator viewportCreator,
            IDataLoaderFactory dataLoaderFactory,
            IScriptMan scriptMan,
            ILogger logger)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.triggerMan = triggerMan;
            this.clipMan = clipMan;
            this.entityFactory = entityFactory;
            this.eventsMan = eventsMan;
            this.collisionMan = collisionMan;
            this.jobsMan = jobsMan;
            this.viewportCreator = viewportCreator;
            this.dataLoaderFactory = dataLoaderFactory;
            this.scriptMan = scriptMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity AddMapEntry(IWorld world, int x, int y, int entryId, string level, int gfxValue)
        {
            var entryEntity = entityFactory.Create(@"ABTA\Templates\Common\MapEntry")
                .SetParameter("level", level)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("entryId", entryId)
                .SetParameter("startX", 16 * x)
            .SetParameter("startY", 16 * y)
                .Build();

            worldMan.RequestAddEntity(entryEntity, world.Id);

            return entryEntity;
        }

        public IEntity AddMapExit(IWorld world, int ix, int iy, int exitId, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\MapExit")
                .SetParameter("level", level)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("exitId", exitId)
                .SetParameter("startX", 16 * ix)
                .SetParameter("startY", 16 * iy)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);
            return entity;
        }

        public void RegisterCollisionPairs()
        {
            //collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.WorldExitTrigger, (ca, ea, cb, eb, pv ) => Actor2TriggerCallback(ca, ea, cb,eb, pv));
            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.WorldExitTrigger, (ca, ea, cb, eb, dt, pv) => Actor2TriggerCallbackEx(ca, ea, cb, eb, dt, pv));

            //collisionMan.RegisterCollisionPair(ColliderTypes.WorldExitTrigger, ColliderTypes.ActorBody, Actor2TriggerCallback);
        }

        public void ExecuteHeroEnter(IEntity heroEntity, IEntity cameraEntity, string worldName, int entryId)
        {
            var context = new Context()
            {
                actorEntity = heroEntity,
                cameraEntity = cameraEntity,
                //cameraFadeInClipId = cameraFadeInClipId,
                //cameraFadeOutClipId = cameraFadeOutClipId,
                mapKey = worldName,
                entryId = entryId
            };

            AddToWorld(context);
        }

        #endregion Public Methods

        #region Private Methods

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

            var cameraFadeOutClipId = clipMan.GetByName(CameraHelper.CAMERA_FADE_OUT).Id;
            var cameraFadeInClipId = clipMan.GetByName(CameraHelper.CAMERA_FADE_IN).Id;

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

        class Context
        {
            public IEntity cameraEntity { get; set; }
            public IEntity actorEntity { get; set; }
            public int cameraFadeOutClipId { get; set; }
            public int cameraFadeInClipId { get; set; }
            public string mapKey { get; set; }
            public int entryId { get; set; }
            public IWorld targetWorld { get; internal set; }

            public Context Then(Func<Context, Context> function)
            {
                return function.Invoke(this);
            }

        }

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

        private Context RemoveFromWorld(Context context)
        {
            triggerMan.OnEntityLeftWorld(context.actorEntity, (s,a) =>
            {
                LoadWorld(context);
            }, singleTime: true);

            worldMan.RequestRemoveEntity(context.actorEntity);

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

        private void Actor2TriggerCallbackEx(BodyFixture colliderTypeA, IEntity entityA, BodyFixture colliderTypeB, IEntity entityB, float dt, Vector2 projection)
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

                if(pos.Y == topMostPosY)
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

            if(entryEntity is null)
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
    }
}
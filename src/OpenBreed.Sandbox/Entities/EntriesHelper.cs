using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Linq;

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
        private readonly IClipMan<Entity> clipMan;

        private readonly IEntityFactory entityFactory;

        private readonly IEventsMan eventsMan;

        private readonly ICollisionMan<Entity> collisionMan;

        private readonly IJobsMan jobsMan;

        private readonly ViewportCreator viewportCreator;
        private readonly IDataLoaderFactory dataLoaderFactory;

        #endregion Private Fields

        #region Public Constructors

        public EntriesHelper(IWorldMan worldMan, IEntityMan entityMan, ITriggerMan triggerMan, IClipMan<Entity> clipMan, IEntityFactory entityFactory, IEventsMan eventsMan, ICollisionMan<Entity> collisionMan, IJobsMan jobsMan, ViewportCreator viewportCreator, IDataLoaderFactory dataLoaderFactory)
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
        }

        #endregion Public Constructors

        #region Public Methods

        public Entity AddMapEntry(World world, int x, int y, int entryId, string level, int gfxValue)
        {
            var entryEntity = entityFactory.Create(@"Defaults\Templates\ABTA\Common\MapEntry.xml")
                .SetParameter("level", level)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("entryId", entryId)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            entryEntity.EnterWorld(world.Id);

            return entryEntity;
        }

        public void AddMapExit(World world, int ix, int iy, int exitId, string level, int gfxValue)
        {
            var exitEntity = entityFactory.Create(@"Defaults\Templates\ABTA\Common\MapExit.xml")
                .SetParameter("level", level)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("exitId", exitId)
                .SetParameter("startX", 16 * ix)
                .SetParameter("startY", 16 * iy)
                .Build();

            exitEntity.EnterWorld(world.Id);
        }

        public void RegisterCollisionPairs()
        {
            //collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.WorldExitTrigger, (ca, ea, cb, eb, pv ) => Actor2TriggerCallback(ca, ea, cb,eb, pv));
            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.WorldExitTrigger, (ca, ea, cb, eb, pv) => Actor2TriggerCallbackEx(ca, ea, cb, eb, pv));

            //collisionMan.RegisterCollisionPair(ColliderTypes.WorldExitTrigger, ColliderTypes.ActorBody, Actor2TriggerCallback);
        }

        public void ExecuteHeroEnter(Entity heroEntity, int worldId, int entryId)
        {
            //var cameraEntity = heroEntity.TryGet<FollowerComponent>()?.FollowerIds.
            //                                                                  Select(item => core.GetManager<IEntityMan>().GetById(item)).
            //                                                                  FirstOrDefault(item => item.Tag is "PlayerCamera");

            //if (cameraEntity == null)
            //    return;

            var jobChain = new JobChain();

            //Add entity to next world
            jobChain.Equeue(new EntityJob<EntityEnteredEventArgs>(triggerMan, heroEntity, () => heroEntity.EnterWorld(worldId)));
            //Set position of entity to entry position in next world
            jobChain.Equeue(new InstantJob(() => SetPosition(heroEntity, entryId, true)));
            //Unpause this world
            //jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post(new PauseWorldCommand(worldIdToRemoveFrom, false))));
            //Fade in camera
            //jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => core.Commands.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0))));

            jobsMan.Execute(jobChain);
        }

        #endregion Public Methods

        #region Private Methods

        private void PerformEntityExit(Entity actorEntity, Entity exitEntity)
        {
            // For preventing running rest of the code when actor will hit couple of teleporter blocks at same time
            if (Equals(actorEntity.State, "Exiting"))
                return;

            actorEntity.State = "Exiting";

            var cameraEntity = actorEntity.TryGet<FollowedComponent>()?.FollowerIds.
                                                                              Select(item => entityMan.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "PlayerCamera");

            if (cameraEntity == null)
                return;

            var matadataCmp = exitEntity.Get<MetadataComponent>();

            if (!int.TryParse(matadataCmp.Flavor, out int exitId))
                throw new InvalidOperationException("Expected exit number");

            var mapId = exitId % 64;
            var entryId = exitId / 64;

            var mapKey = $"Vanilla/{mapId}";

            var jobChain = new JobChain();

            var cameraFadeOutClipId = clipMan.GetByName(CameraHelper.CAMERA_FADE_OUT).Id;
            var cameraFadeInClipId = clipMan.GetByName(CameraHelper.CAMERA_FADE_IN).Id;

            var worldIdToRemoveFrom = actorEntity.WorldId;


            var actorWorld = worldMan.GetById(actorEntity.WorldId);
            var cameraWorld = worldMan.GetById(cameraEntity.WorldId);
            //var doorOpening = PerformFunction(() => door.TryOpen(key));
            //var doorClosing = doorOpening.OnFinishResult((result) => result == "Matching").PerformAction(() => door.Close())
            //door.Wait(5).OnFinish((door) => door.Close()) 
            //door.Close()


            //var jobBuilder = jobsMan.Create();

            //jobBuilder.DoAction(() => cameraEntity.PauseWorld())
            //          .OnFinish().DoAction(() => cameraEntity.PlayAnimation(0, cameraFadeOutClipId))
            //          .OnFinish().DoAction(() => actorEntity.LeaveWorld())
            //          .OnFinish().DoAction(() => TryLoadWorld(mapKey))
            //          .OnFinish().DoAction(() => AddToWorld(actorEntity, mapKey))
            //          .OnFinish().DoAction(() => SetPosition(actorEntity, entryId, true))
            //          .OnFinish().DoAction(() => cameraEntity.PlayAnimation(0, cameraFadeInClipId));

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
            //    .Then(RemoveFromWorld);
        }

        class Context
        {
            public Entity cameraEntity { get; set; }
            public Entity actorEntity { get; set; }
            public int cameraFadeOutClipId { get; set; }
            public int cameraFadeInClipId { get; set; }
            public string mapKey { get; set; }
            public int entryId { get; set; }
            public World targetWorld { get; internal set; }

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
            triggerMan.OnEntityLeftWorld(context.actorEntity, () =>
            {
                var jobChain = new JobChain();
                jobChain.Equeue(new InstantJob(() => LoadWorld(context)));
                jobsMan.Execute(jobChain);

                //LoadWorld(context);
            }, singleTime: true);

            context.actorEntity.LeaveWorld();

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
            triggerMan.OnEntityEnteredWorld(context.actorEntity, () =>
            {
                SetPosition(context);
            }, singleTime: true);

            AddToWorld(context.actorEntity, context.mapKey);
        }

        private void SetPosition(Context context)
        {
            SetPosition(context.actorEntity, context.entryId, true);

            FadeIn(context);
        }

        private void FadeIn(Context context)
        {
            triggerMan.OnEntityEnteredWorld(context.cameraEntity, () =>
            {
                context.cameraEntity.PlayAnimation(0, context.cameraFadeInClipId);
            }, singleTime: true);
        }


        //private void Actor2TriggerCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        //{
        //    if (colliderTypeA == ColliderTypes.WorldExitTrigger && colliderTypeB == ColliderTypes.ActorBody)
        //        PerformEntityExit(entityB, entityA);
        //    else if (colliderTypeA == ColliderTypes.ActorBody && colliderTypeB == ColliderTypes.WorldExitTrigger)
        //        PerformEntityExit(entityA, entityB);
        //}

        private void Actor2TriggerCallbackEx(BodyFixture colliderTypeA, Entity entityA, BodyFixture colliderTypeB, Entity entityB, Vector2 projection)
        {
            //if (colliderTypeA == ColliderTypes.WorldExitTrigger && colliderTypeB == ColliderTypes.ActorBody)
            //    PerformEntityExit(entityB, entityA);
            //else if (colliderTypeA == ColliderTypes.ActorBody && colliderTypeB == ColliderTypes.WorldExitTrigger)
            PerformEntityExit(entityA, entityB);
        }

        private void AddToWorld(Entity target, string worldName)
        {
            var world = worldMan.GetByName(worldName);
            target.EnterWorld(world.Id);
        }

        private World TryLoadWorld(string worldName)
        {
            var world = worldMan.GetByName(worldName);

            if (world is null)
            {
                var mapWorldDataLoader = dataLoaderFactory.GetLoader<MapLegacyDataLoader>();
                world = mapWorldDataLoader.Load(worldName);
            }

            return world;
        }

        private Entity FindEntryEntity(World world, int entryId)
        {
            foreach (var entity in world.Entities.Where(e => e.Contains<MetadataComponent>()))
            {
                var cmpClass = entity.Get<MetadataComponent>();

                if (cmpClass.Name != "WorldEntry")
                    continue;

                if (cmpClass.Flavor != entryId.ToString())
                    continue;

                return entity;
            }

            return null;
        }

        private void SetPosition(Entity target, int entryId, bool cancelMovement)
        {
            var world = worldMan.GetById(target.WorldId);

            var entryEntity = FindEntryEntity(world, entryId);

            if(entryEntity == null)
                entryEntity = FindEntryEntity(world, 2);

            if (entryEntity == null)
                throw new Exception($"No entry with ID '{entryId}' found.");

            var broadphase = world.GetModule<IBroadphaseDynamic>();

            var entryPos = entryEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();
            //var targetAabb = broadphase.GetAabb(target.Id);
            //var offset = new Vector2((32 - targetAabb.Width) / 2.0f, (32 - targetAabb.Height) / 2.0f);

            var newPosition = entryPos.Value;// + offset;

            targetPos.Value = newPosition;

            if (cancelMovement)
            {
                var velocityCmp = target.Get<VelocityComponent>();
                velocityCmp.Value = Vector2.Zero;

                var thrustCmp = target.Get<ThrustComponent>();
                thrustCmp.Value = Vector2.Zero;

                var walkingControlCmp = target.Get<WalkingControlComponent>();
                walkingControlCmp.Direction = new Vector2(0, 0);
            }

            target.State = null;
        }

        #endregion Private Methods
    }
}
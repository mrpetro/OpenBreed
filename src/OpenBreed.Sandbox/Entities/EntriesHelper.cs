using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
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

        private readonly IClipMan clipMan;

        private readonly IEntityFactory entityFactory;

        private readonly IEventsMan eventsMan;

        private readonly ICollisionMan collisionMan;

        private readonly IJobsMan jobsMan;

        private readonly ViewportCreator viewportCreator;
        private readonly IDataLoaderFactory dataLoaderFactory;

        #endregion Private Fields

        #region Public Constructors

        public EntriesHelper(IWorldMan worldMan, IEntityMan entityMan, IClipMan clipMan, IEntityFactory entityFactory, IEventsMan eventsMan, ICollisionMan collisionMan, IJobsMan jobsMan, ViewportCreator viewportCreator, IDataLoaderFactory dataLoaderFactory)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
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
            jobChain.Equeue(new WorldJob<EntityEnteredEventArgs>(worldMan, eventsMan, (s, a) => { return worldMan.GetById(a.WorldId).Id == worldId; }, () => heroEntity.EnterWorld(worldId)));
            //Set position of entity to entry position in next world
            jobChain.Equeue(new EntityJob(() => SetPosition(heroEntity, entryId, true)));
            //Unpause this world
            //jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post(new PauseWorldCommand(worldIdToRemoveFrom, false))));
            //Fade in camera
            //jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => core.Commands.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0))));

            jobsMan.Execute(jobChain);
        }

        #endregion Public Methods

        #region Private Methods

        private void PerformEntityExit(Entity targetEntity, Entity exitEntity)
        {
            var cameraEntity = targetEntity.TryGet<FollowedComponent>()?.FollowerIds.
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

            var worldIdToRemoveFrom = targetEntity.WorldId;

            //Pause this world
            jobChain.Equeue(new WorldJob<WorldPausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => worldMan.GetById(worldIdToRemoveFrom).Pause()));
            //Fade out camera
            jobChain.Equeue(new EntityJob<AnimFinishedEventArgs>(cameraEntity, () => cameraEntity.PlayAnimation(0, cameraFadeOutClipId)));
            //Remove entity from this world
            jobChain.Equeue(new WorldJob<EntityLeftEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => targetEntity.LeaveWorld()));
            //Load next world if needed
            jobChain.Equeue(new EntityJob(() => TryLoadWorld(mapKey)));
            //Add entity to next world
            jobChain.Equeue(new WorldJob<EntityEnteredEventArgs>(worldMan, eventsMan, (s, a) => { return worldMan.GetById(a.WorldId).Name == mapKey; }, () => AddToWorld(targetEntity, mapKey)));
            //Set position of entity to entry position in next world
            jobChain.Equeue(new EntityJob(() => SetPosition(targetEntity, entryId, true)));
            //Unpause this world
            jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => worldMan.GetById(worldIdToRemoveFrom).Unpause()));
            //Fade in camera
            jobChain.Equeue(new EntityJob<AnimFinishedEventArgs>(cameraEntity, () => cameraEntity.PlayAnimation(0, cameraFadeInClipId)));

            jobsMan.Execute(jobChain);
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

        private void TryLoadWorld(string worldName)
        {
            var world = worldMan.GetByName(worldName);

            if (world == null)
            {
                var mapWorldDataLoader = dataLoaderFactory.GetLoader<MapWorldDataLoader>();
                mapWorldDataLoader.Load(worldName);
            }
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
        }

        #endregion Private Methods
    }
}
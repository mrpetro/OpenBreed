using OpenBreed.Core;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Jobs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Events;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Physics.Generic;
using OpenBreed.Common.Tools;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Physics.Interface;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Events;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Wecs.Extensions;

namespace OpenBreed.Sandbox.Entities.WorldGate
{
    public struct WorldGatePair : IEquatable<WorldGatePair>
    {
        public int Id;

        public bool Equals(WorldGatePair other)
        {
            return Id == other.Id;
        }
    }

    public class WorldGateHelper
    {
        public WorldGateHelper(IWorldMan worldMan, IEntityMan entityMan, IEntityFactory entityFactory, IEventsMan eventsMan, ICommandsMan commandsMan, ICollisionMan collisionMan, IJobsMan jobsMan, ViewportCreator viewportCreator)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.entityFactory = entityFactory;
            this.eventsMan = eventsMan;
            this.commandsMan = commandsMan;
            this.collisionMan = collisionMan;
            this.jobsMan = jobsMan;
            this.viewportCreator = viewportCreator;
        }

        public const string SPRITE_WORLD_ENTRY = "Atlases/Sprites/World/Entry";
        public const string SPRITE_WORLD_EXIT = "Atlases/Sprites/World/Exit";
        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly IEntityFactory entityFactory;
        private readonly IEventsMan eventsMan;
        private readonly ICommandsMan commandsMan;
        private readonly ICollisionMan collisionMan;
        private readonly IJobsMan jobsMan;
        private readonly ViewportCreator viewportCreator;

        #region Public Methods

        public Entity AddWorldExit(World world, int x, int y, string worldName, int entryId)
        {
            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\WorldGate\WorldGateExit.xml");
            var teleportEntity = entityFactory.Create(entityTemplate);

            teleportEntity.Tag = (worldName, entryId);
            teleportEntity.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            teleportEntity.EnterWorld(world.Id);

            return teleportEntity;
        }

        public void RegisterCollisionPairs()
        {
            //collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.WorldExitTrigger, (ca, ea, cb, eb, pv ) => Actor2TriggerCallback(ca, ea, cb,eb, pv));
            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.WorldExitTrigger, (ca, ea, cb, eb, pv) => Actor2TriggerCallbackEx(ca, ea, cb, eb, pv));

            //collisionMan.RegisterCollisionPair(ColliderTypes.WorldExitTrigger, ColliderTypes.ActorBody, Actor2TriggerCallback);
        }

        private void PerformEntityExit(Entity targetEntity, Entity exitEntity)
        {
            var cameraEntity = targetEntity.TryGet<FollowerComponent>()?.FollowerIds.
                                                                              Select(item => entityMan.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "PlayerCamera");

            if (cameraEntity == null)
                return;

            var exitInfo = ((string WorldName, int EntryId))exitEntity.Tag;

            var jobChain = new JobChain();

            var worldIdToRemoveFrom = targetEntity.WorldId;

            //Pause this world
            jobChain.Equeue(new WorldJob<WorldPausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => worldMan.GetById(worldIdToRemoveFrom).Pause()));
            //Fade out camera
            jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => commandsMan.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_OUT, 0))));
            //Remove entity from this world
            jobChain.Equeue(new WorldJob<EntityRemovedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => targetEntity.LeaveWorld() ));
            //Load next world if needed
            jobChain.Equeue(new EntityJob(() => TryLoadWorld(exitInfo.WorldName, exitInfo.EntryId)));
            //Add entity to next world
            jobChain.Equeue(new WorldJob<EntityAddedEventArgs>(worldMan, eventsMan, (s, a) => { return worldMan.GetById(a.WorldId).Name == exitInfo.WorldName; }, () => AddToWorld(targetEntity, exitInfo.WorldName)));
            //Set position of entity to entry position in next world
            jobChain.Equeue(new EntityJob(() => SetPosition(targetEntity, exitInfo.EntryId, true)));
            //Unpause this world
            jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => worldMan.GetById(worldIdToRemoveFrom).Unpause()));
            //Fade in camera
            jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => commandsMan.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0))));

            jobsMan.Execute(jobChain);
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
            jobChain.Equeue(new WorldJob<EntityAddedEventArgs>(worldMan, eventsMan, (s, a) => { return worldMan.GetById(a.WorldId).Id == worldId; }, () => AddToWorld(heroEntity, worldId)));
            //Set position of entity to entry position in next world
            jobChain.Equeue(new EntityJob(() => SetPosition(heroEntity, entryId, true)));
            //Unpause this world
            //jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post(new PauseWorldCommand(worldIdToRemoveFrom, false))));
            //Fade in camera
            //jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => core.Commands.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0))));

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

        public Entity AddWorldEntry(World world, int x, int y, int entryId)
        {
            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\WorldGate\WorldGateEntry.xml");
            var teleportEntity = entityFactory.Create(entityTemplate);

            teleportEntity.Tag = new WorldGatePair() { Id = entryId };
            teleportEntity.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            teleportEntity.EnterWorld(world.Id);

            return teleportEntity;
        }

        #endregion Public Methods

        #region Private Methods

        //private static void OnCollision(object sender, CollisionEventArgs args)
        //{
        //    var entity = (Entity)sender;
        //    var core = entity.Core;
        //    var exitEntity = entity;
        //    var targetEntity = args.Entity;

        //    var cameraEntity = targetEntity.TryGet<FollowerComponent>()?.FollowerIds.
        //                                                                      Select(item => core.GetManager<IEntityMan>().GetById(item)).
        //                                                                      FirstOrDefault(item => item.Tag is "PlayerCamera");

        //    if (cameraEntity == null)
        //        return;

        //    var exitInfo = ((string WorldName, int EntryId))exitEntity.Tag;

        //    var jobChain = new JobChain();

        //    var worldIdToRemoveFrom = targetEntity.World.Id;

        //    //Pause this world
        //    jobChain.Equeue(new WorldJob<WorldPausedEventArgs>(core.GetManager<IWorldMan>(), (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post(new PauseWorldCommand(worldIdToRemoveFrom, true))));
        //    //Fade out camera
        //    jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => core.Commands.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_OUT, 0))));
        //    //Remove entity from this world
        //    jobChain.Equeue(new WorldJob<EntityRemovedEventArgs>(core.GetManager<IWorldMan>(), (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post(new RemoveEntityCommand(targetEntity.World.Id, targetEntity.Id))));
        //    //Load next world if needed
        //    jobChain.Equeue(new EntityJob(() => TryLoadWorld(core, exitInfo.WorldName, exitInfo.EntryId)));
        //    //Add entity to next world
        //    jobChain.Equeue(new WorldJob<EntityAddedEventArgs>(core.GetManager<IWorldMan>(), (s, a) => { return core.GetManager<IWorldMan>().GetById(a.WorldId).Name == exitInfo.WorldName; }, () => AddToWorld(targetEntity, exitInfo.WorldName, exitInfo.EntryId)));
        //    //Set position of entity to entry position in next world
        //    jobChain.Equeue(new EntityJob(() => SetPosition(targetEntity, exitInfo.EntryId, true)));
        //    //Unpause this world
        //    jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(core.GetManager<IWorldMan>(), (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post( new PauseWorldCommand(worldIdToRemoveFrom, false))));
        //    //Fade in camera
        //    jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => core.Commands.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0))));

        //    exitEntity.Core.Jobs.Execute(jobChain);
        //}

        private void AddToWorld(Entity target, string worldName)
        {
            var world = worldMan.GetByName(worldName);
            target.EnterWorld(world.Id);
        }

        private void AddToWorld(Entity target, int worldId)
        {
            target.EnterWorld(worldId);
        }

        private void TryLoadWorld(string worldName, int entryId)
        {
            var world = worldMan.GetByName(worldName);

            if (world == null)
            {

            }
        }

        private void SetPosition(Entity target, int entryId, bool cancelMovement)
        {
            var pair = new WorldGatePair() { Id = entryId };

            var entryEntity = entityMan.GetByTag(pair).FirstOrDefault();

            if (entryEntity == null)
                throw new Exception($"No entry with id '{pair.Id}' found.");


            var world = worldMan.GetById(target.WorldId);
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
            }
        }

        #endregion Private Methods
    }
}

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
        public WorldGateHelper(ICore core, IWorldMan worldMan, IEntityMan entityMan, ViewportCreator viewportCreator)
        {
            this.core = core;
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.viewportCreator = viewportCreator;
        }

        public const string SPRITE_WORLD_ENTRY = "Atlases/Sprites/World/Entry";
        public const string SPRITE_WORLD_EXIT = "Atlases/Sprites/World/Exit";
        private readonly ICore core;
        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly ViewportCreator viewportCreator;

        #region Public Methods

        public Entity AddWorldExit(World world, int x, int y, string worldName, int entryId)
        {
            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\WorldGate\WorldGateExit.xml");
            var teleportEntity = core.GetManager<IEntityFactory>().Create(entityTemplate);


            teleportEntity.Tag = (worldName, entryId);

            teleportEntity.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            teleportEntity.Add(new CollisionComponent(ColliderTypes.WorldExitTrigger));
            //teleportEntity.Subscribe<CollisionEventArgs>(OnCollision);

            core.Commands.Post(new AddEntityCommand(world.Id, teleportEntity.Id));
            //world.AddEntity(teleportEntity);

            return teleportEntity;
        }

        public void RegisterCollisionPairs()
        {
            var collisionMan = core.GetManager<ICollisionMan>();

            collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.WorldExitTrigger, (ca, ea, cb, eb, pv ) => Actor2TriggerCallback(ca, ea, cb,eb, pv));
            //collisionMan.RegisterCollisionPair(ColliderTypes.WorldExitTrigger, ColliderTypes.ActorBody, Actor2TriggerCallback);
        }

        private void PerformEntityExit(Entity targetEntity, Entity exitEntity)
        {
            var cameraEntity = targetEntity.TryGet<FollowerComponent>()?.FollowerIds.
                                                                              Select(item => core.GetManager<IEntityMan>().GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "PlayerCamera");

            if (cameraEntity == null)
                return;

            var exitInfo = ((string WorldName, int EntryId))exitEntity.Tag;

            var jobChain = new JobChain();

            var worldIdToRemoveFrom = targetEntity.WorldId;

            var worldMan = core.GetManager<IWorldMan>();
            var eventsMan = core.GetManager<IEventsMan>();


            //Pause this world
            jobChain.Equeue(new WorldJob<WorldPausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post(new PauseWorldCommand(worldIdToRemoveFrom, true))));
            //Fade out camera
            jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => core.Commands.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_OUT, 0))));
            //Remove entity from this world
            jobChain.Equeue(new WorldJob<EntityRemovedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post(new RemoveEntityCommand(targetEntity.WorldId, targetEntity.Id))));
            //Load next world if needed
            jobChain.Equeue(new EntityJob(() => TryLoadWorld(exitInfo.WorldName, exitInfo.EntryId)));
            //Add entity to next world
            jobChain.Equeue(new WorldJob<EntityAddedEventArgs>(worldMan, eventsMan, (s, a) => { return core.GetManager<IWorldMan>().GetById(a.WorldId).Name == exitInfo.WorldName; }, () => AddToWorld(targetEntity, exitInfo.WorldName)));
            //Set position of entity to entry position in next world
            jobChain.Equeue(new EntityJob(() => SetPosition(targetEntity, exitInfo.EntryId, true)));
            //Unpause this world
            jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post(new PauseWorldCommand(worldIdToRemoveFrom, false))));
            //Fade in camera
            jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => core.Commands.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0))));

            core.Jobs.Execute(jobChain);
        }

        public void ExecuteHeroEnter(Entity heroEntity, int worldId, int entryId)
        {
            //var cameraEntity = heroEntity.TryGet<FollowerComponent>()?.FollowerIds.
            //                                                                  Select(item => core.GetManager<IEntityMan>().GetById(item)).
            //                                                                  FirstOrDefault(item => item.Tag is "PlayerCamera");

            //if (cameraEntity == null)
            //    return;

            var eventsMan = core.GetManager<IEventsMan>();

            var jobChain = new JobChain();

            //Add entity to next world
            jobChain.Equeue(new WorldJob<EntityAddedEventArgs>(worldMan, eventsMan, (s, a) => { return core.GetManager<IWorldMan>().GetById(a.WorldId).Id == worldId; }, () => AddToWorld(heroEntity, worldId)));
            //Set position of entity to entry position in next world
            jobChain.Equeue(new EntityJob(() => SetPosition(heroEntity, entryId, true)));
            //Unpause this world
            //jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => core.Commands.Post(new PauseWorldCommand(worldIdToRemoveFrom, false))));
            //Fade in camera
            //jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => core.Commands.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0))));

            core.Jobs.Execute(jobChain);
        }

        private void Actor2TriggerCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            if (colliderTypeA == ColliderTypes.WorldExitTrigger && colliderTypeB == ColliderTypes.ActorBody)
                PerformEntityExit(entityB, entityA);
            else if (colliderTypeA == ColliderTypes.ActorBody && colliderTypeB == ColliderTypes.WorldExitTrigger)
                PerformEntityExit(entityA, entityB);
        }

        public Entity AddWorldEntry(World world, int x, int y, int entryId)
        {
            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\WorldGate\WorldGateEntry.xml");
            var teleportEntity = core.GetManager<IEntityFactory>().Create(entityTemplate);

            teleportEntity.Tag = new WorldGatePair() { Id = entryId };
            teleportEntity.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            core.Commands.Post(new AddEntityCommand(world.Id, teleportEntity.Id));
            //world.AddEntity(teleportEntity);

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
            var world = core.GetManager<IWorldMan>().GetByName(worldName);
            core.Commands.Post(new AddEntityCommand(world.Id, target.Id));
        }

        private void AddToWorld(Entity target, int worldId)
        {
            var world = core.GetManager<IWorldMan>().GetById(worldId);
            core.Commands.Post(new AddEntityCommand(world.Id, target.Id));
        }

        private void TryLoadWorld(string worldName, int entryId)
        {
            var world = core.GetManager<IWorldMan>().GetByName(worldName);

            if (world == null)
            {
                using (var reader = new TxtFileWorldReader(core, worldMan, entityMan, viewportCreator, $".\\Content\\Maps\\{worldName}.txt"))
                    world = reader.GetWorld();
            }
        }

        private void SetPosition(Entity target, int entryId, bool cancelMovement)
        {
            var pair = new WorldGatePair() { Id = entryId };

            var entryEntity = core.GetManager<IEntityMan>().GetByTag(pair).FirstOrDefault();

            if (entryEntity == null)
                throw new Exception($"No entry with id '{pair.Id}' found.");


            var entryPos = entryEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();
            var targetAabb = target.Get<BodyComponent>().Aabb;
            var offset = new Vector2((32 - targetAabb.Width) / 2.0f, (32 - targetAabb.Height) / 2.0f);

            var newPosition = entryPos.Value + offset;

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

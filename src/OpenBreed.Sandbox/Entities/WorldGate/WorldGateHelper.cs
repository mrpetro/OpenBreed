using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Commands;
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
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Sandbox.Worlds;

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
        public const string SPRITE_WORLD_ENTRY = "Atlases/Sprites/World/Entry";
        public const string SPRITE_WORLD_EXIT = "Atlases/Sprites/World/Exit";

        #region Public Methods

        public static IEntity AddWorldExit(World world, int x, int y, string worldName, int entryId)
        {
            var core = world.Core;

            var teleportEntity = core.Entities.CreateFromTemplate("WorldGateExit");

            teleportEntity.Tag = (worldName, entryId);

            teleportEntity.GetComponent<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            teleportEntity.Subscribe<CollisionEventArgs>(OnCollision);

            world.PostCommand(new AddEntityCommand(world.Id, teleportEntity.Id));
            //world.AddEntity(teleportEntity);

            return teleportEntity;
        }

        public static IEntity AddWorldEntry(World world, int x, int y, int entryId)
        {
            var core = world.Core;
            var teleportEntity = core.Entities.CreateFromTemplate("WorldGateEntry");
            teleportEntity.Tag = new WorldGatePair() { Id = entryId };
            teleportEntity.GetComponent<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            world.PostCommand(new AddEntityCommand(world.Id, teleportEntity.Id));
            //world.AddEntity(teleportEntity);

            return teleportEntity;
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnCollision(object sender, CollisionEventArgs args)
        {
            var entity = (IEntity)sender;
            var exitEntity = entity;
            var targetEntity = args.Entity;

            var cameraEntity = targetEntity.Tag as IEntity;

            if (cameraEntity == null)
                return;

            var exitInfo = ((string WorldName, int EntryId))exitEntity.Tag;

            var jobChain = new JobChain();


            var worldToRemoveFrom = cameraEntity.World;
            var job = new WorldJob<WorldPausedEventArgs>((s, a) => { return a.World == worldToRemoveFrom; }, cameraEntity.Core.Worlds, () => cameraEntity.PostCommand(new PauseWorldCommand(worldToRemoveFrom.Id, true)));


            jobChain.Equeue(job);

            //jobChain.Equeue(new WorldJob(cameraEntity.World, "Pause"));
            jobChain.Equeue(new EntityJobEx<AnimStoppedEventArgs>(cameraEntity, new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_OUT, 0)));


            jobChain.Equeue(new WorldJob<EntityRemovedEventArgs>((s, a) =>
            { 
                return cameraEntity.Core.Worlds.GetById(a.WorldId) == worldToRemoveFrom;
            },
                cameraEntity.Core.Worlds, () => cameraEntity.PostCommand( new RemoveEntityCommand(worldToRemoveFrom.Id, cameraEntity.Id))));
            jobChain.Equeue(new WorldJob<EntityRemovedEventArgs>((s, a) => { return targetEntity.Core.Worlds.GetById(a.WorldId) == worldToRemoveFrom; }, targetEntity.Core.Worlds, () => targetEntity.PostCommand(new RemoveEntityCommand(targetEntity.World.Id, targetEntity.Id))));

            jobChain.Equeue(new EntityJobEx2(cameraEntity, () => TryLoadWorld(cameraEntity, exitInfo.WorldName, exitInfo.EntryId)));
            //jobChain.Equeue(new EntityJobEx2(targetEntity, () => SetPosition(targetEntity, exitInfo.Item2)));

            var job2 = new WorldJob<EntityAddedEventArgs>((s, a) => { return cameraEntity.Core.Worlds.GetById(a.WorldId).Name == exitInfo.WorldName; }, cameraEntity.Core.Worlds, () => AddToWorld(cameraEntity, exitInfo.WorldName, exitInfo.EntryId));
            //job2.CompleteTrigger<EntityAddedEventArgs>();

            jobChain.Equeue(job2);

            //jobChain.Equeue(new WorldJobEx2<EntityAddedEventArgs>(cameraEntity.Core, exitInfo.WorldName, () => AddToWorld(cameraEntity, exitInfo.WorldName, exitInfo.EntryId)));
            jobChain.Equeue(new WorldJob<EntityAddedEventArgs>((s, a) => { return cameraEntity.Core.Worlds.GetById(a.WorldId).Name == exitInfo.WorldName; }, cameraEntity.Core.Worlds, () => AddToWorld(targetEntity, exitInfo.WorldName, exitInfo.EntryId)));





            jobChain.Equeue(new EntityJobEx2(cameraEntity, () => SetPosition(cameraEntity, exitInfo.EntryId)));
            jobChain.Equeue(new EntityJobEx2(targetEntity, () => SetPosition(targetEntity, exitInfo.EntryId)));

            jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>((s, a) => { return a.World == worldToRemoveFrom; }, cameraEntity.Core.Worlds, () => cameraEntity.PostCommand( new PauseWorldCommand(worldToRemoveFrom.Id, false))));

            jobChain.Equeue(new EntityJobEx<AnimStoppedEventArgs>(cameraEntity, new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0)));

            exitEntity.Core.Jobs.Execute(jobChain);
        }

        private static void AddToWorld(IEntity target, string worldName, int entryId)
        {
            var world = target.Core.Worlds.GetByName(worldName);
            world.PostCommand(new AddEntityCommand(world.Id, target.Id));
        }

        private static void TryLoadWorld(IEntity target, string worldName, int entryId)
        {
            var world = target.Core.Worlds.GetByName(worldName);

            if (world == null)
            {
                using (var reader = new TxtFileWorldReader(target.Core, $".\\Content\\Maps\\{worldName}.txt"))
                    world = reader.GetWorld();
            }
        }

        private static void SetPosition(IEntity target, int entryId)
        {
            var pair = new WorldGatePair() { Id = entryId };

            var entryEntity = target.Core.Entities.GetByTag(pair).FirstOrDefault();

            if (entryEntity == null)
                throw new Exception($"No entry with id '{pair.Id}' found.");

            var entryPos = entryEntity.GetComponent<PositionComponent>();
            var entryAabb = entryEntity.GetComponent<BodyComponent>().Aabb;
            //var entityAabb = entity.GetComponent<IShapeComponent>().First().Aabb;
            var entityPos = target.GetComponent<PositionComponent>();

            //var offset = new Vector2((32 - entityAabb.Width) / 2.0f, (32 - entityAabb.Height) / 2.0f);

            entityPos.Value = entryPos.Value;// + offset;
        }

        #endregion Private Methods
    }
}

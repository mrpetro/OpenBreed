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

            teleportEntity.Tag = new Tuple<string, int>(worldName, entryId);

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

            var exitInfo = (Tuple<string, int>)exitEntity.Tag;

            var jobChain = new JobChain();
            jobChain.Equeue(new WorldJobEx<WorldPausedEventArgs>(cameraEntity.World, new PauseWorldCommand(cameraEntity.World.Id, true)));
            //jobChain.Equeue(new WorldJob(cameraEntity.World, "Pause"));
            jobChain.Equeue(new EntityJobEx<AnimStoppedEventArgs>(cameraEntity, new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_OUT, 0)));
            //jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_OUT));

            //jobChain.Equeue(new EntityJobEx<EntityLeftWorldEventArgs>(cameraEntity, new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0)));
            //entity.Subscribe<EntityLeftWorldEventArgs>(OnEntityLeftWorld);
            //entity.World.PostCommand(new RemoveEntityCommand(entity.World.Id, entity.Id));

            //var job = new WorldJobEx<EntityRemovedEventArgs>(cameraEntity.World, new RemoveEntityCommand(cameraEntity.World.Id, cameraEntity.Id));



            jobChain.Equeue(new WorldJobEx<EntityRemovedEventArgs>(cameraEntity.World, new RemoveEntityCommand(cameraEntity.World.Id, cameraEntity.Id)));
            jobChain.Equeue(new WorldJobEx<EntityRemovedEventArgs>(targetEntity.World, new RemoveEntityCommand(targetEntity.World.Id, targetEntity.Id)));

            //jobChain.Equeue(new EntityJob(cameraEntity, "LeaveWorld"));
            //jobChain.Equeue(new EntityJob(targetEntity, "LeaveWorld"));

            jobChain.Equeue(new EntityJobEx2(cameraEntity, () => TryCreateWorld(cameraEntity, exitInfo.Item1, exitInfo.Item2)));
            //jobChain.Equeue(new EntityJobEx2(targetEntity, () => SetPosition(targetEntity, exitInfo.Item2)));

            //jobChain.Equeue(new EntityJobEx2<EntityAddedEventArgs>(cameraEntity, () => AddToWorld(cameraEntity, exitInfo.Item1, exitInfo.Item2)));
            //jobChain.Equeue(new EntityJobEx2<EntityAddedEventArgs>(targetEntity, () => AddToWorld(targetEntity, exitInfo.Item1, exitInfo.Item2)));

            jobChain.Equeue(new EntityJob(cameraEntity, "EnterWorld", exitInfo.Item1, exitInfo.Item2));
            jobChain.Equeue(new EntityJob(targetEntity, "EnterWorld", exitInfo.Item1, exitInfo.Item2));

            //jobChain.Equeue(new WorldJobEx<EntityAddedEventArgs>(cameraEntity.World, new AddEntityCommand(cameraEntity.World.Id, cameraEntity.Id)));
            //jobChain.Equeue(new WorldJobEx<EntityAddedEventArgs>(targetEntity.World, new AddEntityCommand(targetEntity.World.Id, targetEntity.Id)));


            jobChain.Equeue(new EntityJobEx2(cameraEntity, () => SetPosition(cameraEntity, exitInfo.Item2)));
            jobChain.Equeue(new EntityJobEx2(targetEntity, () => SetPosition(targetEntity, exitInfo.Item2)));
            //jobChain.Equeue(new EntityJob(cameraEntity, "EnterWorld", exitInfo.Item1, exitInfo.Item2));
            //jobChain.Equeue(new EntityJob(targetEntity, "EnterWorld", exitInfo.Item1, exitInfo.Item2));

            //jobChain.Equeue(new EntityJob(cameraEntity, "SetPosition", exitInfo.Item1, exitInfo.Item2));
            //jobChain.Equeue(new EntityJob(targetEntity, "SetPosition", exitInfo.Item1, exitInfo.Item2));

            jobChain.Equeue(new WorldJobEx<WorldUnpausedEventArgs>(cameraEntity.World, new PauseWorldCommand(cameraEntity.World.Id, false)));
            //jobChain.Equeue(new TeleportJob(targetEntity, exitPos.Value + offset, true));
            jobChain.Equeue(new EntityJobEx<AnimStoppedEventArgs>(cameraEntity, new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0)));
            //jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_IN));

            exitEntity.Core.Jobs.Execute(jobChain);
        }

        private static void AddToWorld(IEntity target, string worldName, int entryId)
        {
            var world = target.Core.Worlds.GetByName(worldName);
            world.PostCommand(new AddEntityCommand(world.Id, target.Id));
        }

        private static void TryCreateWorld(IEntity target, string worldName, int entryId)
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

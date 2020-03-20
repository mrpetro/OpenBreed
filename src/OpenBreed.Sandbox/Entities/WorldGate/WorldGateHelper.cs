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
            teleportEntity.Subscribe<AnimChangedEventArgs>(OnFrameChanged);

            world.AddEntity(teleportEntity);

            return teleportEntity;
        }

        private static void OnFrameChanged(object sender, AnimChangedEventArgs e)
        {
            var entity = (IEntity)sender;
            var sprite = entity.GetComponent<SpriteComponent>();
            sprite.ImageId = (int)e.Frame;
        }

        public static IEntity AddWorldEntry(World world, int x, int y, int entryId)
        {
            var core = world.Core;
            var teleportEntity = core.Entities.CreateFromTemplate("WorldGateEntry");
            teleportEntity.Tag = new WorldGatePair() { Id = entryId };
            teleportEntity.GetComponent<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            teleportEntity.Subscribe<AnimChangedEventArgs>(OnFrameChanged);

            world.AddEntity(teleportEntity);

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

            jobChain.Equeue(new WorldJob(cameraEntity.World, "Pause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_OUT));

            jobChain.Equeue(new EntityJob(cameraEntity, "LeaveWorld"));
            jobChain.Equeue(new EntityJob(targetEntity, "LeaveWorld"));

            jobChain.Equeue(new EntityJob(cameraEntity, "EnterWorld", exitInfo.Item1, exitInfo.Item2));
            jobChain.Equeue(new EntityJob(targetEntity, "EnterWorld", exitInfo.Item1, exitInfo.Item2));

            //jobChain.Equeue(new TeleportJob(targetEntity, exitPos.Value + offset, true));
            jobChain.Equeue(new WorldJob(cameraEntity.World, "Unpause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_IN));

            exitEntity.Core.Jobs.Execute(jobChain);
        }



        #endregion Private Methods
    }
}

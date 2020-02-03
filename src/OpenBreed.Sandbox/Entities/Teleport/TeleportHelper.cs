using OpenBreed.Core;
using OpenBreed.Core.Common;

using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Jobs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Modules.Physics.Events;

namespace OpenBreed.Sandbox.Entities.Teleport
{
    public struct TeleportPair : IEquatable<TeleportPair>
    {
        public int Id;

        public bool Equals(TeleportPair other)
        {
            return Id == other.Id;
        }
    }

    public static class TeleportHelper
    {
        #region Public Fields

        public const string SPRITE_TELEPORT_ENTRY = "Atlases/Sprites/Teleport/Entry";

        #endregion Public Fields

        #region Private Fields

        private const string ANIMATION_TELEPORT_ENTRY = "Animations/Teleport/Entry";

        #endregion Private Fields

        #region Public Methods

        public static void CreateAnimations(ICore core)
        {
            var animationTeleportEntry = core.Animations.Anims.Create<int>(ANIMATION_TELEPORT_ENTRY);
            animationTeleportEntry.AddFrame(0, 1.0f);
            animationTeleportEntry.AddFrame(1, 1.0f);
            animationTeleportEntry.AddFrame(2, 1.0f);
            animationTeleportEntry.AddFrame(3, 1.0f);
        }

        public static IEntity AddTeleportEntry(World world, int x, int y, int pairId)
        {
            var core = world.Core;

            var teleportEntry = core.Entities.CreateFromTemplate("TeleportEntry");

            teleportEntry.Tag = new TeleportPair { Id = pairId };
            teleportEntry.Add(AxisAlignedBoxShape.Create(16, 16, 8, 8));
            teleportEntry.Add(TextHelper.Create(core, new Vector2(0, 32), "TeleportEntry"));

            teleportEntry.Components.OfType<Position>().First().Value = new Vector2( 16 * x, 16 * y);

            teleportEntry.Subscribe(AnimationEventTypes.ANIMATION_CHANGED, (s, a) => OnFrameChanged((IEntity)s, (AnimChangedEventArgs)a));
            teleportEntry.Subscribe(PhysicsEventTypes.COLLISION_OCCURRED, (s, a) => OnCollision((IEntity)s, (CollisionEventArgs)a));
            world.AddEntity(teleportEntry);

            return teleportEntry;
        }

        public static IEntity AddTeleportExit(World world, int x, int y, int pairId)
        {
            var core = world.Core;

            var teleportExit = core.Entities.CreateFromTemplate("TeleportExit");

            teleportExit.Tag = new TeleportPair { Id = pairId };
            teleportExit.Add(AxisAlignedBoxShape.Create(16, 16, 8, 8));
            teleportExit.Add(TextHelper.Create(core, new Vector2(0, 32), "TeleportExit"));

            teleportExit.Components.OfType<Position>().First().Value = new Vector2(16 * x, 16 * y);

            teleportExit.Subscribe(AnimationEventTypes.ANIMATION_CHANGED, (s,a) => OnFrameChanged((IEntity)s, (AnimChangedEventArgs)a));

            world.AddEntity(teleportExit);

            return teleportExit;
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnFrameChanged(IEntity entity, AnimChangedEventArgs systemEvent)
        {
            var sprite = entity.Components.OfType<SpriteComponent>().First();
            sprite.ImageId = (int)systemEvent.Frame;
        }

        private static void OnCollision(IEntity entity, CollisionEventArgs args)
        {
            var entryEntity = entity;
            var targetEntity = args.Entity;

            var cameraEntity = targetEntity.Tag as IEntity;

            if (cameraEntity == null)
                return;

            var pair = (TeleportPair)entryEntity.Tag;

            var exitEntity = entryEntity.Core.Entities.GetByTag(pair).FirstOrDefault(item => item != entryEntity);

            if (exitEntity == null)
                throw new Exception("No exit entity found");

            var exitPos = exitEntity.Components.OfType<Position>().First();
            var entryAabb = entryEntity.Components.OfType<IShapeComponent>().First().Aabb;
            var targetAabb = targetEntity.Components.OfType<IShapeComponent>().First().Aabb;
            var offset = new Vector2((32 - targetAabb.Width) / 2.0f, (32 - targetAabb.Height) / 2.0f);

            //Vanilla game
            //1. Pause game
            //2. Camera fade-out
            //3. Teleport character
            //4. Unpause game
            //5. Camera fade-in

            var jobChain = new JobChain();
            //jobChain.Equeue(new EntityJob(entryEntity, "BodyOff"));
            jobChain.Equeue(new WorldJob(cameraEntity.World, "Pause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_OUT));
            jobChain.Equeue(new TeleportJob(targetEntity, exitPos.Value + offset, true));
            jobChain.Equeue(new WorldJob(cameraEntity.World, "Unpause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_IN));
            //jobChain.Equeue(new EntityJob(entryEntity, "BodyOn"));

            entryEntity.Core.Jobs.Execute(jobChain);
        }



        #endregion Private Methods
    }
}
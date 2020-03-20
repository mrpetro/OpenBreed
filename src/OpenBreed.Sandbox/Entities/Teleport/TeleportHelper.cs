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
        public const string SPRITE_TELEPORT_EXIT = "Atlases/Sprites/Teleport/Exit";

        #endregion Public Fields

        #region Private Fields

        private const string ANIMATION_TELEPORT_ENTRY = "Animations/Teleport/Entry";
        private const string ANIMATION_TELEPORT_EXIT = "Animations/Teleport/Exit";

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

            teleportEntry.GetComponent<PositionComponent>().Value = new Vector2( 16 * x, 16 * y);

            teleportEntry.Subscribe<AnimChangedEventArgs>(OnFrameChanged);
            teleportEntry.Subscribe<CollisionEventArgs>(OnCollision);
            world.AddEntity(teleportEntry);

            return teleportEntry;
        }

        public static IEntity AddTeleportExit(World world, int x, int y, int pairId)
        {
            var core = world.Core;

            var teleportExit = core.Entities.CreateFromTemplate("TeleportExit");

            teleportExit.Tag = new TeleportPair { Id = pairId };

            teleportExit.GetComponent<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            teleportExit.Subscribe<AnimChangedEventArgs>(OnFrameChanged);

            world.AddEntity(teleportExit);

            return teleportExit;
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnFrameChanged(object sender, AnimChangedEventArgs e)
        {
            var entity = (IEntity)sender;
            var sprite = entity.GetComponent<SpriteComponent>();
            sprite.ImageId = (int)e.Frame;
        }

        private static void OnCollision(object sender, CollisionEventArgs args)
        {
            var entity = (IEntity)sender;
            var entryEntity = entity;
            var targetEntity = args.Entity;

            var cameraEntity = targetEntity.Tag as IEntity;

            if (cameraEntity == null)
                return;

            var pair = (TeleportPair)entryEntity.Tag;

            var exitEntity = entryEntity.Core.Entities.GetByTag(pair).FirstOrDefault(item => item != entryEntity);

            if (exitEntity == null)
                throw new Exception("No exit entity found");

            var exitPos = exitEntity.GetComponent<PositionComponent>();
            var entryAabb = entryEntity.GetComponent<BodyComponent>().Aabb;
            var targetAabb = targetEntity.GetComponent<BodyComponent>().Aabb;
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
using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Teleport
{
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

        public static IEntity AddTeleportEntry(ICore core, World world, int x, int y, int exitEntityId)
        {
            var anim = core.Animations.Anims.GetByName(ANIMATION_TELEPORT_ENTRY);

            var teleportEntry = core.Entities.Create();

            teleportEntry.Add(new Animator(10.0f, true, anim.Id));
            teleportEntry.Add(Body.Create(1.0f, 1.0f, "Trigger", (e, c) => OnCollision(teleportEntry, e, c)));
            teleportEntry.Add(core.Rendering.CreateSprite(SPRITE_TELEPORT_ENTRY));
            teleportEntry.Add(Position.Create(x * 16, y * 16));
            teleportEntry.Add(AxisAlignedBoxShape.Create(16, 16, 8, 8));
            teleportEntry.Add(TextHelper.Create(core, new Vector2(-10, 10), "TeleportEntry"));
            teleportEntry.Subscribe(AnimChangedEvent.TYPE, OnTeleportFrameChanged);
            teleportEntry.Tag = exitEntityId;

            world.AddEntity(teleportEntry);

            return teleportEntry;
        }

        public static IEntity AddTeleportExit(ICore core, World world, int x, int y)
        {
            var anim = core.Animations.Anims.GetByName(ANIMATION_TELEPORT_ENTRY);

            var teleportEntity = core.Entities.Create();

            teleportEntity.Add(new Animator(10.0f, true, anim.Id));
            teleportEntity.Add(core.Rendering.CreateSprite(SPRITE_TELEPORT_ENTRY));
            teleportEntity.Add(Position.Create(x * 16, y * 16));
            teleportEntity.Add(AxisAlignedBoxShape.Create(16, 16, 8, 8));
            teleportEntity.Add(TextHelper.Create(core, new Vector2(-10, 10), "TeleportExit"));
            teleportEntity.Subscribe(AnimChangedEvent.TYPE, OnTeleportFrameChanged);

            world.AddEntity(teleportEntity);

            return teleportEntity;
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnTeleportFrameChanged(object sender, IEvent e)
        {
            HandleTeleportFrameChangeEvent((IEntity)sender, (AnimChangedEvent)e);
        }

        private static void HandleTeleportFrameChangeEvent(IEntity entity, AnimChangedEvent systemEvent)
        {
            var sprite = entity.Components.OfType<ISpriteComponent>().First();
            sprite.ImageId = (int)systemEvent.Frame;
        }

        private static void OnCollision(IEntity thisEntity, IEntity otherEntity, Vector2 projection)
        {
            var cameraEntity = otherEntity.Tag as IEntity;

            if (cameraEntity == null)
                return;

            var exitEntityId = (int)thisEntity.Tag;
            var existEntity = thisEntity.Core.Entities.GetById(exitEntityId);
            var exitPos = existEntity.Components.OfType<Position>().First();
            var thisAabb = thisEntity.Components.OfType<IShapeComponent>().First().Aabb;
            var otherAabb = otherEntity.Components.OfType<IShapeComponent>().First().Aabb;
            var offset = new Vector2((32 - otherAabb.Width) / 2.0f, (32 - otherAabb.Height) / 2.0f);

            //Vanilla game
            //1. Pause game
            //2. Camera fade-out
            //3. Teleport character
            //4. Unpause game
            //5. Camera fade-in

            var jobChain = new JobChain();
  
            jobChain.Equeue(new WorldJob(cameraEntity.World, "Pause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_OUT));
            jobChain.Equeue(new TeleportJob(otherEntity, exitPos.Value + offset, true));
            jobChain.Equeue(new WorldJob(cameraEntity.World, "Unpause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_IN));

            thisEntity.Core.Jobs.Execute(jobChain);
        }



        #endregion Private Methods
    }
}
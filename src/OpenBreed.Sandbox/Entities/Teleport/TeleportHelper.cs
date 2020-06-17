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
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Events;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Sandbox.Entities.WorldGate;

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
            var animationTeleportEntry = core.Animations.Create<int>(ANIMATION_TELEPORT_ENTRY, OnFrameUpdate);
            animationTeleportEntry.AddFrame(0, 1.0f);
            animationTeleportEntry.AddFrame(1, 1.0f);
            animationTeleportEntry.AddFrame(2, 1.0f);
            animationTeleportEntry.AddFrame(3, 1.0f);

        }

        private static void OnFrameUpdate(IEntity entity, int nextValue)
        {
            entity.PostCommand(new SpriteSetCommand(entity.Id, nextValue));
        }

        public static IEntity AddTeleportEntry(World world, int x, int y, int pairId)
        {
            var core = world.Core;

            var teleportEntry = core.Entities.CreateFromTemplate("TeleportEntry");

            teleportEntry.Tag = new TeleportPair { Id = pairId };

            teleportEntry.GetComponent<PositionComponent>().Value = new Vector2( 16 * x, 16 * y);

            //teleportEntry.Subscribe<AnimChangedEventArgs>(OnFrameChanged);
            teleportEntry.Subscribe<CollisionEventArgs>(OnCollision);
            world.PostCommand(new AddEntityCommand(world.Id, teleportEntry.Id));
            //world.AddEntity(teleportEntry);

            return teleportEntry;
        }

        public static IEntity AddTeleportExit(World world, int x, int y, int pairId)
        {
            var core = world.Core;

            var teleportExit = core.Entities.CreateFromTemplate("TeleportExit");

            teleportExit.Tag = new TeleportPair { Id = pairId };

            teleportExit.GetComponent<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            //teleportExit.Subscribe<AnimChangedEventArgs>(OnFrameChanged);

            world.PostCommand(new AddEntityCommand(world.Id, teleportExit.Id));
            //world.AddEntity(teleportExit);

            return teleportExit;
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnCollision(object sender, CollisionEventArgs args)
        {
            var entity = (IEntity)sender;
            var entryEntity = entity;
            var world = entity.World;
            var targetEntity = args.Entity;
            var core = targetEntity.Core;

            var cameraEntity = targetEntity.TryGetComponent<FollowedComponent>()?.FollowerIds.
                                                                              Select(item => core.Entities.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "PlayerCamera");

            if (cameraEntity == null)
                return;

            //Vanilla game
            var jobChain = new JobChain();
            //1. Pause game
            jobChain.Equeue(new WorldJob<WorldPausedEventArgs>((s, a) => { return a.WorldId == world.Id; }, core.Worlds, () => cameraEntity.PostCommand(new PauseWorldCommand(cameraEntity.World.Id, true))));
            //2. Camera fade-out   
            jobChain.Equeue(new EntityJobEx<AnimStoppedEventArgs>(cameraEntity, new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_OUT, 0)));
            //3. Teleport character
            jobChain.Equeue(new EntityJobEx2(targetEntity, () => SetPosition(targetEntity, entryEntity, true)));
            //4. Unpause game
            jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>((s, a) => { return a.WorldId == world.Id; }, core.Worlds, () => cameraEntity.PostCommand(new PauseWorldCommand(cameraEntity.World.Id, false))));
            //5. Camera fade-in
            jobChain.Equeue(new EntityJobEx<AnimStoppedEventArgs>(cameraEntity, new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0)));

            entryEntity.Core.Jobs.Execute(jobChain);
        }

        public static void SetPosition(IEntity target, IEntity entryEntity, bool cancelMovement)
        {
            var pair = (TeleportPair)entryEntity.Tag;
            var exitEntity = target.Core.Entities.GetByTag(pair).FirstOrDefault(item => item != entryEntity);

            if (exitEntity == null)
                throw new Exception("No exit entity found");

            var exitPos = exitEntity.GetComponent<PositionComponent>();
            var targetPos = target.GetComponent<PositionComponent>();
            var entryAabb = entryEntity.GetComponent<BodyComponent>().Aabb;
            var targetAabb = target.GetComponent<BodyComponent>().Aabb;
            var offset = new Vector2((32 - targetAabb.Width) / 2.0f, (32 - targetAabb.Height) / 2.0f);

            var newPosition = exitPos.Value + offset;

            targetPos.Value = newPosition;

            if (cancelMovement)
            {
                var velocityCmp = target.GetComponent<VelocityComponent>();
                velocityCmp.Value = Vector2.Zero;

                var thrustCmp = target.GetComponent<ThrustComponent>();
                thrustCmp.Value = Vector2.Zero;
            }

        }

        #endregion Private Methods
    }
}
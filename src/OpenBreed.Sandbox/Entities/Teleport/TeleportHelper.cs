﻿using OpenBreed.Core;

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
using OpenBreed.Core.Components;
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
            var animationTeleportEntry = core.Animations.Create(ANIMATION_TELEPORT_ENTRY, 4.0f);
            var te = animationTeleportEntry.AddPart<int>(OnFrameUpdate, 0);
            te.AddFrame(0, 1.0f);
            te.AddFrame(1, 2.0f);
            te.AddFrame(2, 3.0f);
            te.AddFrame(3, 4.0f);

        }

        private static void OnFrameUpdate(Entity entity, int nextValue)
        {
            entity.Core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        public static Entity AddTeleportEntry(World world, int x, int y, int pairId)
        {
            var core = world.Core;

            var teleportEntry = core.Entities.CreateFromTemplate("TeleportEntry");

            teleportEntry.Tag = new TeleportPair { Id = pairId };

            teleportEntry.Get<PositionComponent>().Value = new Vector2( 16 * x, 16 * y);
            teleportEntry.Add(new CollisionComponent(ColliderTypes.TeleportEntryTrigger));
            world.Core.Commands.Post(new AddEntityCommand(world.Id, teleportEntry.Id));
            return teleportEntry;
        }

        public static Entity AddTeleportExit(World world, int x, int y, int pairId)
        {
            var core = world.Core;

            var teleportExit = core.Entities.CreateFromTemplate("TeleportExit");

            teleportExit.Tag = new TeleportPair { Id = pairId };

            teleportExit.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            //teleportExit.Subscribe<AnimChangedEventArgs>(OnFrameChanged);

            world.Core.Commands.Post(new AddEntityCommand(world.Id, teleportExit.Id));
            //world.AddEntity(teleportExit);

            return teleportExit;
        }

        #endregion Public Methods

        #region Private Methods

        public static void SetPosition(Entity target, Entity entryEntity, bool cancelMovement)
        {
            var pair = (TeleportPair)entryEntity.Tag;
            var exitEntity = target.Core.Entities.GetByTag(pair).FirstOrDefault(item => item != entryEntity);

            if (exitEntity == null)
                throw new Exception("No exit entity found");

            var exitPos = exitEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();
            var targetAabb = target.Get<BodyComponent>().Aabb;
            var offset = new Vector2((32 - targetAabb.Width) / 2.0f, (32 - targetAabb.Height) / 2.0f);

            var newPosition = exitPos.Value + offset;

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
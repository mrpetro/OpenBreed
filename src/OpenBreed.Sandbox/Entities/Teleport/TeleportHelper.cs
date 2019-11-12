using OpenBreed.Core;
using OpenBreed.Core.Blueprints;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Components;
using OpenBreed.Sandbox.Components.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Teleport
{
    public static class TeleportHelper
    {
        public const string SPRITE_TELEPORT_ENTRY = "Atlases/Sprites/Teleport/Entry";
        private const string ANIMATION_TELEPORT_ENTRY = "Animations/Teleport/Entry";

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
            teleportEntry.Subscribe(AnimChangedEvent.TYPE, OnFrameChanged);
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
            teleportEntity.Subscribe(AnimChangedEvent.TYPE, OnFrameChanged);

            world.AddEntity(teleportEntity);

            return teleportEntity;
        }

        private static void OnFrameChanged(object sender, IEvent e)
        {
            HandleFrameChangeEvent((IEntity)sender, (AnimChangedEvent)e);
        }

        private static void HandleFrameChangeEvent(IEntity entity, AnimChangedEvent systemEvent)
        {
            var sprite = entity.Components.OfType<ISpriteComponent>().First();
            sprite.ImageId = (int)systemEvent.Frame;
        }

        private static void OnCollision(IEntity thisEntity, IEntity otherEntity, Vector2 projection)
        {
            var exitEntityId = (int)thisEntity.Tag;
            var existEntity = thisEntity.Core.Entities.GetById(exitEntityId);
            var exitPos = existEntity.Components.OfType<IPosition>().First();
            otherEntity.Components.OfType<IPosition>().First().Value = exitPos.Value;
        }
    }
}

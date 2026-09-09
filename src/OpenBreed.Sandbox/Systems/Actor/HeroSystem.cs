using Microsoft.Extensions.Logging;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Helpers;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems.Events;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Physics.Systems.Events;
using System;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class HeroSystem : IEventSystem<DestroyedEvent>, IEventSystem<VelocityChangedEvent>, IEventSystem<DirectionChangedEvent>
    {
        private readonly IGameServices services;

        public HeroSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public void OnEvent(
            [EntityOfClassFilter("Hero")]
            DestroyedEvent e, IWorld world)
        {
            var entity = services.Entities.GetById(e.EntityId);

            var pos = entity.GetPosition();
            entity.StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
                .SetOption("flavor", "Small")
                .SetOption("startX", pos.X)
                .SetOption("startY", pos.Y)
                .Finish();

            var soundId = services.Sounds.GetByName("Vanilla/Common/Hero/Dying");
            entity.EmitSound(soundId);

            entity.SetResurrectable(entity.WorldId);

            services.Logger.LogInformation("Player Died!");

            var limboWorld = services.Worlds.GetByName(WorldNames.Limbo);

            entity.State = "Dead";

            var task = Core.Task.Create((t) => services.AddToWorld(t, entity, WorldNames.Limbo));

            task.Then((t) => services.Wait(t, entity, 3000))
                .Then((t) => services.Resurrect(t, entity));

            task.Start();
        }

        public void OnEvent(
            [EntityOfClassFilter("Hero")]
            VelocityChangedEvent e, IWorld world)
        {
            var entity = services.Entities.GetById(e.EntityId);
            var entityClass = services.Classes.GetById(entity.ClassId);
            var className = entityClass.Name;
            var targetDirection = entity.GetTargetDirection();
            var direction = entity.GetDirection();
            var animDirName = AnimHelper.ToDirectionName(direction);

            var isMoving = entity.IsMoving();
            var movementStateName = default(string);

            if (isMoving)
            {
                movementStateName = "Walking";
                var clipId = services.Clips.GetId($"Vanilla/Common/{className}/{movementStateName}/{animDirName}");
                entity.PlayAnimation(0, clipId);
            }
            else
            {
                movementStateName = "Standing";
                var clipId = services.Clips.GetId($"Vanilla/Common/{className}/{movementStateName}/{animDirName}");
                entity.StopAnimation(0);
            }
        }

        public void OnEvent(
            [EntityOfClassFilter("Hero")]
            DirectionChangedEvent e, IWorld world)
        {
            var entity = services.Entities.GetById(e.EntityId);
            var entityClass = services.Classes.GetById(entity.ClassId);
            var className = entityClass.Name;

            var targetDirection = entity.GetTargetDirection();
            var direction = entity.GetDirection();
            var animDirName = AnimHelper.ToDirectionName(direction);

            var isMoving = entity.IsMoving();
            var movementStateName = default(string);

            if (isMoving)
            {
                movementStateName = "Walking";
            }
            else
            {
                movementStateName = "Standing";
            }

            var clipId = services.Clips.GetId($"Vanilla/Common/{className}/{movementStateName}/{animDirName}");
            entity.PlayAnimation(0, clipId);
        }
    }
}
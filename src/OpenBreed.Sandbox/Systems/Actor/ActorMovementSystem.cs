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
    public class ActorMovementSystem : IEventSystem<VelocityChangedEvent>, IEventSystem<DirectionChangedEvent>
    {
        private readonly IGameServices services;

        public ActorMovementSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public void OnEvent(
            [EntityOfClassFilter("MovableActor")]
            VelocityChangedEvent e, IWorld world)
        {
            var entity = services.Entities.GetById(e.EntityId);
            var entityClass = services.Classes.GetById(entity.ClassId);
            var className = entityClass.Name;
            var targetDirection = entity.GetTargetDirection();
            var direction = entity.GetDirection();
            var animDirName = AnimHelper.ToDirectionName(direction);
            var level = entity.GetMetadata("Level");

            if (level is null)
            {
                level = "Vanilla/Common";
            }

            var isMoving = entity.IsMoving();
            var movementStateName = default(string);

            if (isMoving)
            {
                movementStateName = "Walking";
                var clipId = services.Clips.GetId($"{level}/{className}/{movementStateName}/{animDirName}");
                entity.PlayAnimation(0, clipId);
            }
            else
            {
                movementStateName = "Standing";
                var clipId = services.Clips.GetId($"{level}/{className}/{movementStateName}/{animDirName}");
                entity.StopAnimation(0);
            }
        }

        public void OnEvent(
            [EntityOfClassFilter("MovableActor")]
            DirectionChangedEvent e, IWorld world)
        {
            var entity = services.Entities.GetById(e.EntityId);
            var entityClass = services.Classes.GetById(entity.ClassId);
            var className = entityClass.Name;

            var targetDirection = entity.GetTargetDirection();
            var direction = entity.GetDirection();
            var animDirName = AnimHelper.ToDirectionName(direction);
            var level = entity.GetMetadata("Level");

            if (level is null)
            {
                level = "Vanilla/Common";
            }

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

            var clipId = services.Clips.GetId($"{level}/{className}/{movementStateName}/{animDirName}");
            entity.PlayAnimation(0, clipId);
        }
    }
}
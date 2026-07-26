using OpenBreed.Common.Game.Services;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Helpers;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Physics.Systems.Events;
using System;

namespace OpenBreed.Sandbox.Systems.Actor
{
    [RequireEntityWithClass("Hero")]
    public class ActorAnimateSystem : IEventSystem<VelocityChangedEvent>, IEventSystem<DirectionChangedEvent>
    {
        private readonly IGameServices services;
        private readonly IEntityClass heroClass;

        public ActorAnimateSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));

            heroClass = services.Classes.GetByName("Hero");
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
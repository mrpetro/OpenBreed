using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;

using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Physics.Components;
using OpenTK.Mathematics;


namespace OpenBreed.Common.Game.Wecs.Extensions
{
    public static class GameServicesExtension
    {
        #region Public Methods

        public static void PauseWorld(this IGameServices services, ITask task, IEntity entity)
        {
            services.Triggers.OnPausedWorld(entity, (e, a) =>
            {
                task.Finish();
            }, singleTime: true);

            entity.PauseWorld();
        }

        public static void UnpauseWorld(this IGameServices services, ITask task, IEntity entity)
        {
            services.Triggers.OnUnpausedWorld(entity, (e, a) =>
            {
                task.Finish();
            }, singleTime: true);

            entity.UnpauseWorld();
        }

        public static void RemoveFromWorld(this IGameServices services, ITask task, IEntity actorEntity)
        {
            services.Logger.LogTrace("OnExit: Removing entity '{actorEntity}'...", actorEntity);

            services.Triggers.OnEntityLeftWorld(actorEntity, (s, a) =>
            {
                task.Finish();
            }, singleTime: true);

            services.Worlds.RequestRemoveEntity(actorEntity);
        }

        public static void AddToWorld(this IGameServices services, ITask task, IEntity actorEntity, string mapKey)
        {
            services.Logger.LogTrace("OnExit: Adding entity '{actorEntity}' to world '{mapKey}'...", actorEntity, mapKey);

            services.Triggers.OnEntityEnteredWorld(actorEntity, (e, args) =>
            {
                task.Finish();
            }, singleTime: true);

            services.AddToWorld(actorEntity, mapKey);
        }

        public static void AddToWorld(this IGameServices services, IEntity target, string worldName)
        {
            var world = services.Worlds.GetByName(worldName);

            services.Worlds.RequestAddEntity(target, world.Id);
        }

        public static Vector2 GetExitPosition(this IGameServices services, IEntity entryEntity)
        {
            var pairId = entryEntity.Tag.Split('/')[1];
            // Search for all exits from same world as entry with same pair ID 
            var exitEntity = services.Entities.GetByTag($"TeleportExit/{pairId}").FirstOrDefault(item => item.WorldId == entryEntity.WorldId);

            if (exitEntity is null)
                throw new Exception("No exit entity found");

            var exitPos = exitEntity.Get<PositionComponent>();

            return exitPos.Value;
        }

        public static void SetPosition(this IGameServices services, ITask task, IEntity target, Vector2 exitPosition)
        {
            var targetPos = target.Get<PositionComponent>();
            var bodyCmp = target.Get<BodyComponent>();
            var shape = bodyCmp.Fixtures.First().Shape;
            var targetAabb = shape.GetAabb().Translated(targetPos.Value);

            var offset = new Vector2((32 - targetAabb.Size.X) / 2.0f, (32 - targetAabb.Size.Y) / 2.0f);

            var newPosition = exitPosition + offset;

            targetPos.Value = newPosition;

            var velocityCmp = target.Get<VelocityComponent>();
            velocityCmp.Value = Vector2.Zero;

            var thrustCmp = target.Get<ThrustComponent>();
            thrustCmp.Value = Vector2.Zero;

            target.State = null;

            task.Finish();
        }

        #endregion Public Methods
    }
}
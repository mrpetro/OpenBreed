using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Core.Components.Extensions;

using OpenBreed.Wecs.Core.Systems.Extensions;


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

        #endregion Public Methods
    }
}
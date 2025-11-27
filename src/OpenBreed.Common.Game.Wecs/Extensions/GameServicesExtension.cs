using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Common.Game.Wecs.Services;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Interface;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
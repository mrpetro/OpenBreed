using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Services;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Interface;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Systems.Scripting.Extensions;

namespace OpenBreed.Sandbox.Extensions
{
    public static class GameServicesExtension
    {
        public static void FadeOut(this IGameServices services, ITask task, IEntity cameraEntity)
        {
            services.Logger.LogTrace("OnExit: Fade out...");

            var cameraFadeOutClipId = services.Clips.GetId(CameraHelper.CAMERA_FADE_OUT);

            services.Triggers.OnEntityAnimFinished(cameraEntity, (e, a) =>
            {
                task.Finish();
            }, singleTime: true);

            cameraEntity.PlayAnimation(0, cameraFadeOutClipId);
        }

        public static void LoadWorld(this IGameServices services, ITask task, string mapKey)
        {
            services.Logger.LogTrace("OnExit: Loading world '{mapKey}'...", mapKey);

            var targetWorld = services.TryLoadWorld(mapKey);

            services.Triggers.OnWorldInitialized(targetWorld, () =>
            {
                task.Finish();
            }, singleTime: true);
        }

        public static void PlayerCharacterEnter(this IGameServices services, ITask task, IEntity actorEntity, int entryId)
        {
            services.Logger.LogTrace("OnExit: Actor '{actorEntity}' arrives at entry {entryId}...", actorEntity, entryId);

            actorEntity.TryInvoke(services.Scripts, services.Logger, "OnEnter");

            services.Worlds.SetEntityPosition(actorEntity, entryId);

            task.Finish();
        }

        public static IWorld TryLoadWorld(this IGameServices services, string worldName)
        {
            var world = services.Worlds.GetByName(worldName);

            if (world is null)
            {
                var mapWorldDataLoader = services.DataLoaderFactory.GetLoader<MapLegacyDataLoader>();
                world = mapWorldDataLoader.Load(worldName);
            }

            return world;
        }
    }
}

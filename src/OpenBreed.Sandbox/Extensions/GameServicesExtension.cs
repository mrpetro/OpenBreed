using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Animation.Systems.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Scripting.Systems.Extensions;

using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Audio.Systems.Extensions;

namespace OpenBreed.Sandbox.Extensions
{
    public static class GameServicesExtension
    {
        #region Public Methods

        public static void Wait(this IGameServices services, ITask task, IEntity entity, int timeMs)
        {
            services.Logger.LogInformation("Wait {0} seconds...", timeMs);

            services.Triggers.AfterDelay(
                entity,
                TimeSpan.FromMilliseconds(timeMs),
                (e, a) =>
                {
                    task.Finish();
                },
                singleTime: true);
        }

        public static void TextFadeIn(this IGameServices services, ITask task, IEntity entity)
        {
            var textFadeInClipId = services.Clips.GetId("Vanilla/Common/Text/Effects/FadeIn");
            services.PlayAnimation(task, entity, textFadeInClipId, "Text fade in...");
        }

        public static void TextFadeOut(this IGameServices services, ITask task, IEntity entity)
        {
            var textFadeOutClipId = services.Clips.GetId("Vanilla/Common/Text/Effects/FadeOut");
            services.PlayAnimation(task, entity, textFadeOutClipId, "Text fade out...");
        }

        public static void FadeIn(this IGameServices services, ITask task, IEntity cameraEntity)
        {
            var clipId = services.Clips.GetId(CameraHelper.CAMERA_FADE_IN);
            services.PlayAnimation(task, cameraEntity, clipId, "Fade in...");
        }

        public static void FadeOut(this IGameServices services, ITask task, IEntity cameraEntity)
        {
            var clipId = services.Clips.GetId(CameraHelper.CAMERA_FADE_OUT);
            services.PlayAnimation(task, cameraEntity, clipId, "Fade out...");
        }

        public static void WaitForKey(this IGameServices services, ITask task, string taskDescription = null)
        {
            if (string.IsNullOrEmpty(taskDescription))
            {
                services.Logger.LogInformation(taskDescription);
            }

            services.Triggers.AnyKeyPressed((a) =>
            {
                task.Finish();
            }, singleTime: true);
        }

        public static void Say(this IGameServices services, ITask task, IEntity entity, string sampleName)
        {
            services.Logger.LogInformation($"Saying '{sampleName}...'");
            var soundId = services.Sounds.GetByName(sampleName);

            var duration = services.Sounds.GetDuration(soundId);

            entity.EmitSound(soundId);

            services.Triggers.AfterDelay(
                entity,
                TimeSpan.FromMilliseconds(duration), (e, a) => task.Finish(),
                singleTime: true);
        }

        public static void Resurrect(this IGameServices services, ITask task, IEntity entity)
        {
            services.Logger.LogInformation("Resurrect...");
            entity.RestoreFullHealth();
            entity.Resurrect();

            task.Finish();
        }

        public static void PlayAnimation(this IGameServices services, ITask task, IEntity entity, int animationId, string taskDescription = null)
        {
            if (string.IsNullOrEmpty(taskDescription))
            {
                services.Logger.LogInformation(taskDescription);
            }

            services.Triggers.OnEntityAnimFinished(
                entity, (e, a) =>
                {
                    task.Finish();
                },
                singleTime: true);

            entity.PlayAnimation(0, animationId);
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

            //services.EntityTriggers.TryOnTrigger("EnterWorld", actorEntity, actorEntity);

            //actorEntity.TryInvoke(services.Scripts, services.Logger, "OnEnter");

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

        #endregion Public Methods
    }
}
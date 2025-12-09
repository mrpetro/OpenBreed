using Microsoft.Extensions.Logging;
using OpenBreed.Common.Game.Services;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenTK.Compute.OpenCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core.Abstractions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Scripting.Extensions;

namespace OpenBreed.Sandbox.Systems.Mission
{
    internal class MissionShowOnHeroEnterSystem : IEntityOnTriggerActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public MissionShowOnHeroEnterSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "HeroEnter";

        public string ActionName => "Show";

        #endregion Public Properties

        #region Public Methods

        public void OnTrigger(IEntity triggeringEntity, IEntity triggerEntity)
        {
            var playerCharacterEntity = triggeringEntity;
            var missionEntity = triggerEntity;

            var gameCameraEntity = services.Entities.GetPlayerCamera(playerCharacterEntity);
            var missionScreenCameraEntity = services.Entities.GetMissionScreenCamera();
            var missionScreenTextEntity = services.Entities.GetMissionScreenText();
            var missionScreenBackgroundEntity = services.Entities.GetMissionScreenBackground();
            var gameWorld = services.Worlds.GetWorld(playerCharacterEntity);
            var directorEntity = services.Entities.GetDirector(gameWorld.Id);
            var hudCameraEntity = services.Entities.GetHudCamera();
            var hudViewportEntity = services.Entities.GetHudViewport();
            var gameViewportEntity = services.Entities.GetGameViewport();
            var backgroundDarkenClipId = services.Clips.GetId("Vanilla/Common/Picture/Effects/Darken");
            var textFadeInClipId = services.Clips.GetId("Vanilla/Common/Text/Effects/FadeIn");
            var textFadeOutClipId = services.Clips.GetId("Vanilla/Common/Text/Effects/FadeOut");
            var cameraFadeOutClipId = services.Clips.GetId("Vanilla/Common/Camera/Effects/FadeOut");
            var cameraFadeInClipId = services.Clips.GetId("Vanilla/Common/Camera/Effects/FadeIn");
            var missionMetadata = missionEntity.GetMetadata();
            var textId = $"{gameWorld.Name}/{missionMetadata.Name}";
            services.Logger.LogInformation("Text Id: {0}", textId);
            var text = services.Texts.GetTextString(textId);
            var currentCharacter = 0;
            var textLength = text.Length;
            var delayTimer = 0;





            if (Equals(playerCharacterEntity.State, "MissionShowing"))
            {
                return;
            }

            playerCharacterEntity.State = "MissionShowing";

            if (gameCameraEntity is null)
            {
                return;
            }

            missionScreenBackgroundEntity.SetPictureColor(1.0f, 1.0f, 1.0f, 1.0f);
            missionScreenTextEntity.SetTextColor(0, 1.0f, 1.0f, 1.0f, 0.0f);
            missionScreenTextEntity.SetText(0, text);

            var commentator = services.Entities.GetCommentator();



            var task = Core.Task.Create((t) => services.PauseWorld(t, gameCameraEntity));

            task.Then((t) => SwitchViewToMissionWorld(t, gameCameraEntity))
                .Then((t) => services.FadeIn(t, missionScreenCameraEntity))
                .Then((t) => services.Wait(t, commentator, 2000))
                .Then((t) => DarkenBackground(t, missionScreenBackgroundEntity))
                .Then((t) => TextFadeIn(t, missionScreenTextEntity))
                .Then((t) => WaitForKey(t))
                .Then((t) => TextFadeOut(t, missionScreenTextEntity))
                .Then((t) => ScreenFadeOut(t, missionScreenCameraEntity))
                .Then((t) => services.UnpauseWorld(t, gameCameraEntity))
                .Then((t) => SwitchViewToGameWorld(t));

            task.Start();


            void SwitchViewToMissionWorld(ITask task, IEntity entity)
            {
                services.Logger.LogInformation("Switch view to mission world camera...");
                missionScreenCameraEntity.SetBrightness(0);
                hudViewportEntity.SetViewportCamera(missionScreenCameraEntity.Id);

                task.Finish();
            }

            void DarkenBackground(ITask task, IEntity entity)
            {
                services.Logger.LogInformation("Darken background...");

                services.Triggers.OnEntityAnimFinished(
                    entity, (e, a) =>
                    {
                        task.Finish();
                    },
                    singleTime: true);

                entity.PlayAnimation(0, backgroundDarkenClipId);
            }


            void TextFadeIn(ITask task, IEntity entity)
            {
                services.Logger.LogInformation("Text fade in...");

                services.Triggers.OnEntityAnimFinished(
                    entity, (e, a) =>
                    {
                        task.Finish();
                    },
                    singleTime: true);

                entity.PlayAnimation(0, textFadeInClipId);
            }

            void TextFadeOut(ITask task, IEntity entity)
            {
                services.Logger.LogInformation("Text fade out...");

                services.Triggers.OnEntityAnimFinished(
                    entity, (e, a) =>
                    {
                        task.Finish();
                    },
                    singleTime: true);

                entity.PlayAnimation(0, textFadeOutClipId);
            }


            void WaitForKey(ITask task)
            {
                services.Logger.LogInformation("Wait for key...");

                services.Triggers.AnyKeyPressed((a) =>
                    {
                        task.Finish();
                    },
                    singleTime: true);
            }


            void ScreenFadeOut(ITask task, IEntity entity)
            {
                services.Logger.LogInformation("Screen Fade Out...");

                services.Triggers.OnEntityAnimFinished(
                    entity, (e, a) =>
                    {
                        task.Finish();
                    },
                    singleTime: true);

                entity.PlayAnimation(0, cameraFadeOutClipId);
            }

            void SwitchViewToGameWorld(ITask task)
            {
                services.Logger.LogInformation("Switch view to game world camera...");

                gameCameraEntity.SetBrightness(0);
                hudViewportEntity.SetViewportCamera(hudCameraEntity.Id);

                gameCameraEntity.PlayAnimation(0, cameraFadeInClipId);

                playerCharacterEntity.State = null;

                directorEntity.TryInvoke(services.Scripts, services.Logger, "OnStartMission", null);
            }
        }

        #endregion Public Methods
    }
}
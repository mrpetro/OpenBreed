using Microsoft.Extensions.Logging;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core.Abstractions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Animation.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using OpenTK.Compute.OpenCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Abstractions.Systems;

namespace OpenBreed.Sandbox.Systems.Mission
{
    internal class OnActorInitShowMissionSystem : IOnAddEntityActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public OnActorInitShowMissionSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "EnterWorld";

        public string ActionName => "ShowMission";

        #endregion Public Properties

        #region Public Methods

        public void OnAddEntity(IWorld world, IEntity entity)
        {
            var playerCharacterEntity = entity;

            if (Equals(playerCharacterEntity.State, "MissionShowing"))
            {
                return;
            }

            if (Equals(playerCharacterEntity.State, "Dead"))
            {
                return;
            }
            var missionEntity = services.Entities.GetMission(world.Id);

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
            var cameraFadeInClipId = services.Clips.GetId("Vanilla/Common/Camera/Effects/FadeIn");
            var missionMetadata = missionEntity.GetMetadata();
            var textId = $"{gameWorld.Name}/{missionMetadata.Name}";
            services.Logger.LogInformation("Text Id: {0}", textId);
            var text = services.Texts.GetTextString(textId);
            var textLength = text.Length;

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
                .Then((t) => services.PlayAnimation(t, missionScreenBackgroundEntity, backgroundDarkenClipId, "Darken background..."))
                .Then((t) => services.TextFadeIn(t, missionScreenTextEntity))
                .Then((t) => services.WaitForKey(t))
                .Then((t) => services.TextFadeOut(t, missionScreenTextEntity))
                .Then((t) => services.FadeOut(t, missionScreenCameraEntity))
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
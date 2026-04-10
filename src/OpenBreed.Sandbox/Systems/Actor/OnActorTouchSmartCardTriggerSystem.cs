using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Interface;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;
using OpenBreed.Core.Abstractions;
using OpenBreed.Input.Interface.Events;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems.Events;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Windows.Controls;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class OnActorTouchSmartCardTriggerSystem : IOnActorTouchObstacleSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public OnActorTouchSmartCardTriggerSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "ActorTouch";
        public string ActionName => "Read";

        #endregion Public Properties

        #region Public Methods

        public void OnTouch(
            IFixture actorFixture, IEntity actorEntity,
            IFixture triggerFixture, IEntity triggerEntity,
            Vector2 projection)
        {
            var smartCardEntity = triggerEntity;

            var gameCommentator = services.Entities.GetCommentator();
            var gameCameraEntity = services.Entities.GetPlayerCamera(actorEntity);
            var smartCardScreenCameraEntity = services.Entities.GetSmartCardScreenCamera();
            var smartCardScreenTextEntity = services.Entities.GetSmartCardScreenText();
            var gameWorld = services.Worlds.GetWorld(smartCardEntity);
            var hudCameraEntity = services.Entities.GetHudCamera();
            var hudViewportEntity = services.Entities.GetHudViewport();
            var gameViewportEntity = services.Entities.GetGameViewport();
            var cameraFadeOutClipId = services.Clips.GetId("Vanilla/Common/Camera/Effects/FadeOut");
            var cameraFadeInClipId = services.Clips.GetId("Vanilla/Common/Camera/Effects/FadeIn");
            var smartCardMetadata = smartCardEntity.GetMetadata();
            var textId = $"{gameWorld.Name}/{smartCardMetadata.Name}/{smartCardMetadata.Option}";
            services.Logger.LogInformation("Text Id: {0}", textId);
            var text = services.Texts.GetTextString(textId);
            var currentCharacter = 0;
            var textLength = text.Length;

            if (actorEntity.State?.ToString() == "SmartCardReading")
            {
                return;
            }

            actorEntity.State = "SmartCardReading";

            if (gameCameraEntity is null)
            {
                return;
            }

            services.Entities.ForEachEntity(gameWorld.Id,
                "SmartCard",
                smartCardMetadata.Option,
                RemoveSmartcard);

            var soundName = "Vanilla/Common/Speech/SmartCardMessageFollows";
            var soundId = services.Sounds.GetByName(soundName);
            gameCommentator.EmitSound(soundId);

            var task = Core.Task.Create((t) => services.PauseWorld(t, actorEntity));

            task.Then((t) => services.FadeOut(t, gameCameraEntity))
                .Then((t) => SwitchViewToSmartCardWorld(t));
            //    .Then((job) => services.LoadWorld(job, mapKey))
            //    .Then((job) => services.AddToWorld(job, actorEntity, mapKey))
            //    .Then((job) => services.PlayerCharacterEnter(job, actorEntity, entryId));

            task.Start();


            void SwitchViewToSmartCardWorld(ITask task)
            {
                services.Logger.LogInformation("Switch view to smart card world camera...");

                smartCardScreenCameraEntity.SetBrightness(0);
                hudViewportEntity.SetViewportCamera(smartCardScreenCameraEntity.Id);

                services.Triggers.OnEntityAnimFinished(
                        smartCardScreenCameraEntity,
                        SmartCardWorldShowText,
                        true);

                    smartCardScreenCameraEntity.PlayAnimation(0, cameraFadeInClipId);

                services.Triggers.EveryFrame(
                        gameCommentator,
                        DisplayTextWithNextCharacter,
                        0,
                        true);
            }


            void DisplayTextWithNextCharacter(IEntity entity, EntityFrameEvent e)
            {
                if (currentCharacter > textLength)
                {
                    return;
                }

                var textPart = text.Substring(0, currentCharacter);

                smartCardScreenTextEntity.SetText(0, textPart);

                currentCharacter = currentCharacter + 1;

                services.Triggers.EveryFrame(
                    gameCommentator,
                    DisplayTextWithNextCharacter,
                    0,
                    true);
            }

            void SmartCardWorldShowText(IEntity entity, AnimFinishedEvent @event)
            {
                services.Logger.LogInformation("Smart card world show text...");

                services.Triggers.AnyKeyPressed(
                ShowAllTextIfNeeded,
                true);
            }

            void ShowAllTextIfNeeded(KeyDownEvent e)
            {
                //If all text is shown, go straight to fade out
                if (currentCharacter > textLength)
                {
                    SmartCardWorldFadeOut(null);
                    return;
                }

                //Else show all text and wait for button to be pressed
                currentCharacter = textLength;
                services.Triggers.AnyKeyPressed(
                    SmartCardWorldFadeOut,
                    true);
            }

            void SmartCardWorldFadeOut(KeyDownEvent e)
            {
                services.Logger.LogInformation("Smart card world fade out...");

                services.Triggers.OnEntityAnimFinished(
                    smartCardScreenCameraEntity,
                    GameWorldUnpause,
                    true);

                smartCardScreenCameraEntity.PlayAnimation(0, cameraFadeOutClipId);
            }

            void GameWorldUnpause(IEntity entity, AnimFinishedEvent @event)
            {
                services.Logger.LogInformation("Game world unpause...");

                services.Triggers.OnUnpausedWorld(
                gameCameraEntity,
                SwitchViewToGameWorld,
                true);

                gameCameraEntity.UnpauseWorld();
            }

            void SwitchViewToGameWorld(IEntity entity, WorldUnpausedEventArgs args)
            {
                services.Logger.LogInformation("Switch view to game world camera...");

                gameCameraEntity.SetBrightness(0);
                hudViewportEntity.SetViewportCamera(hudCameraEntity.Id);
                //actorEntity.SetPosition(Entities, Shapes, smartCardEntity);
                //services.Triggers.OnEntityAnimFinished(
                //gameCameraEntity,
                //GameWorldUnpause,
                //true);

                gameCameraEntity.PlayAnimation(0, cameraFadeInClipId);
                actorEntity.State = null;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RemoveSmartcard(IEntity entity)
        {
            var mapEntity = services.Entities.GetMapEntity(entity.WorldId);
            var metaData = entity.GetMetadata();

            if (metaData.Flavor != "Trigger")
            {
                var stampName = $"{metaData.Level}/{metaData.Name}/{metaData.Flavor}/Picked";
                services.Logger.LogInformation("StampName: {0}", stampName);
                var stampId = services.Stamps.GetByName(stampName).Id;
                services.Logger.LogInformation("StampId: {0}", stampId);
                mapEntity.PutStampAtEntityPosition(entity, stampId, 0);
            }

            services.Worlds.RequestRemoveEntity(entity);
            services.Entities.RequestErase(entity);
        }

        #endregion Private Methods
    }
}
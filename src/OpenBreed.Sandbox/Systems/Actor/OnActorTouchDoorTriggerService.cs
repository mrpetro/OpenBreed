using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Interface;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using OpenBreed.Wecs.Control.Systems.Events;

namespace OpenBreed.Sandbox.Systems.Actor
{
    internal class OnActorTouchDoorTriggerService : IOnActorTouchObstacleSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public OnActorTouchDoorTriggerService(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "ActorTouch";
        public string ActionName => "TryOpen";

        #endregion Public Properties

        #region Public Methods

        public void OnTouch(
            IFixture actorFixture, IEntity actorEntity,
            IFixture triggerFixture, IEntity triggerEntity,
            Vector2 projection)
        {
            var doorEntity = triggerEntity;

            var metaData = doorEntity.GetMetadata();

            if (metaData.State != null)
            {
                return;
            }

            services.Logger.LogInformation("Door Open!");

            if (metaData.Option != null)
            {
                var keycardItemId = services.Items.GetItemId(metaData.Option);

                if (keycardItemId != -1)
                {
                    services.Logger.LogInformation("KeyCard {0} required.", metaData.Option);

                    var actorInventory = actorEntity.GetInventory();

                    var itemSlot = actorInventory.GetItemSlot(keycardItemId);

                    //No keycard item then door can't be opened
                    if (itemSlot is null)
                    {
                        services.Logger.LogInformation("No KeyCard!");
                        return;
                    }
                }
            }

            var doorCell = services.Entities.FindHorizontalDoorCell(services.Worlds, doorEntity);

            if (doorCell != doorEntity)
            {
                //Logging: Info("Open from right!")
                var nextDoorCell = services.Entities.GetEntityByDataGrid(services.Worlds, doorCell, 1, 0);
                OpenDoor(doorCell, nextDoorCell, "Horizontal");
            }
            else
            {
                if (services.Entities.IsSameCellType(services.Worlds, doorCell, 1, 0))
                {
                    //Logging: Info("Open from left!")
                    var nextDoorCell = services.Entities.GetEntityByDataGrid(services.Worlds, doorCell, 1, 0);
                    OpenDoor(doorCell, nextDoorCell, "Horizontal");
                }
                else
                {
                    //Single cell horizontally, might be vertical Door
                    doorCell = services.Entities.FindVerticalDoorCell(services.Worlds, doorEntity);
                    var nextDoorCell = services.Entities.GetEntityByDataGrid(services.Worlds, doorCell, 0, 1);
                    OpenDoor(doorCell, nextDoorCell, "Vertical");
                }
            }

            void OpenDoor(IEntity doorCell, IEntity nextDoorCell, string flavor)
            {
                services.Logger.LogInformation("Opening {0} Door...", flavor);

                var mapEntity = services.Entities.GetMapEntity(doorCell.WorldId);

                var clipName = $"{metaData.Level}/{metaData.Name}/Opening/{flavor}";
                var stampName = $"{metaData.Level}/{metaData.Name}/{flavor}/Opened";
                var soundName = $"Vanilla/Common/{metaData.Name}/Opening";

                var clipId = services.Clips.GetId(clipName);
                var stampId = services.Stamps.GetByName(stampName).Id;
                var soundId = services.Sounds.GetByName(soundName);

                doorCell.SetSpriteOn();
                doorCell.PlayAnimation(0, clipId);
                mapEntity.PutStampAtEntityPosition(doorCell, stampId, 0);
                doorCell.EmitSound(soundId);

                if (flavor == "Horizontal")
                {
                    services.Triggers.OnEntityAnimFinished(
                        doorCell,
                        RemoveHorizontalDoorObstacle,
                        true);
                }
                else
                {
                    services.Triggers.OnEntityAnimFinished(
                        doorCell,
                        RemoveVerticalDoorObstacle,
                        true);
                }

                var doorCellMeta = doorCell.GetMetadata();
                doorCellMeta.State = "Opening";

                var nextDoorCellMeta = nextDoorCell.GetMetadata();
                nextDoorCellMeta.State = "Opening";
            }

            void RemoveHorizontalDoorObstacle(IEntity doorCell, AnimFinishedEvent e)
            {
                services.Logger.LogInformation("Removing Horizontal Door Obstacle...");

                doorCell.SetSpriteOff();
                doorCell.SetBodyOffEx();

                var doorCellMeta = doorCell.GetMetadata();
                doorCellMeta.State = "Opened";

                var nextDoorCell = services.Entities.GetEntityByDataGrid(services.Worlds, doorCell, 1, 0);

                nextDoorCell.SetBodyOffEx();

                var nextDoorCellMeta = nextDoorCell.GetMetadata();
                nextDoorCellMeta.State = "Opened";

                services.Entities.RequestErase(doorCell);
                services.Entities.RequestErase(nextDoorCell);
            }

            void RemoveVerticalDoorObstacle(IEntity doorCell, AnimFinishedEvent e)
            {
                services.Logger.LogInformation("Removing Vertical Door Obstacle...");

                doorCell.SetSpriteOff();
                doorCell.SetBodyOffEx();

                var doorCellMeta = doorCell.GetMetadata();
                doorCellMeta.State = "Opened";

                var nextDoorCell = services.Entities.GetEntityByDataGrid(services.Worlds, doorCell, 0, 1);
                nextDoorCell.SetBodyOffEx();

                var nextDoorCellMeta = nextDoorCell.GetMetadata();
                nextDoorCellMeta.State = "Opened";

                services.Entities.RequestErase(doorCell);
                services.Entities.RequestErase(nextDoorCell);
            }
        }

        #endregion Public Methods
    }
}
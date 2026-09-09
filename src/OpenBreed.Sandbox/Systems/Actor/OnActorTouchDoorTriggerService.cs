using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Abstractions;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Systems.Actor;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public enum DoorState
    {
        Closed,
        LockChecking,
        Locked,
        Opening,

        Open
    }

    public enum DoorImpulse
    {
        EntityTouched,
        KeyFound,
        KeyNotFound,
        OpeningFinished
    }

    public class ImpulseEvent : EntityEvent
    {
        #region Public Constructors

        public ImpulseEvent(int entityId, IFsmImpulse impulse) : base(entityId)
        {
            Impulse = impulse;
        }

        #endregion Public Constructors

        #region Private Properties

        private IFsmImpulse Impulse { get; }

        #endregion Private Properties
    }

    public record TouchImpulse(IEntity Actor) : IFsmImpulse;

    public record OpenDoorImpulse(IEntity Actor) : IFsmImpulse;
    public record UseImpulse(IEntity Actor) : IFsmImpulse;

    public record UnlockImpulse(IEntity Actor) : IFsmImpulse;

    public record FinishImpulse(IEntity Actor) : IFsmImpulse;

    //Closed + OpenDoor       → Opening
    //Opening + DoorOpened    → Open
    //Opening + DoorBlocked   → Closed
    //Open + CloseDoor        → Closing
    //Closing + DoorClosed    → Closed


    public class DoorComponent : IEntityComponent
    {
        #region Public Properties

        public DoorState State { get; set; }

        #endregion Public Properties
    }

    internal class OnActorTouchDoorTriggerService : IOnActorTouchObstacleSystem
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly IEventsMan eventsMan;
        private readonly IFsmMachine<IEntity, DoorState> machine;

        #endregion Private Fields

        #region Public Constructors

        public OnActorTouchDoorTriggerService(IGameServices services, IEventsMan eventsMan, IFsmMachineFactory<IEntity> fsmMachineFactory)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.machine = fsmMachineFactory.GetMachine<DoorState>()
                .Register<TouchImpulse>(DoorState.Locked, DoorState.Locked, DoorLockedToLocked)
                .Register<UnlockImpulse>(DoorState.Locked, DoorState.Closed, DoorLockedToClosed)
                .Register<TouchImpulse>(DoorState.Closed, DoorState.LockChecking, CheckLock)
                .Register<OpenDoorImpulse>(DoorState.LockChecking, DoorState.Opening, StartOpening)
                .Register<FinishImpulse>(DoorState.Opening, DoorState.Open, OpeningToOpen);
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
            Console.WriteLine("Touched:" + triggerEntity);

            var doorEntity = triggerEntity;
            machine.Process(doorEntity, new TouchImpulse(actorEntity));
        }

        #endregion Public Methods

        #region Private Methods

        private void OpeningToOpen(IEntity context, FinishImpulse impulse, DoorState from, DoorState to)
        {
            //throw new NotImplementedException();
        }

        private void StartOpening(IEntity context, OpenDoorImpulse impulse, DoorState from, DoorState to)
        {
            var doorEntity = context;

            var metaData = doorEntity.GetMetadata();
            var className = services.Classes.GetById(doorEntity.ClassId).Name;
            var actorEntity = impulse.Actor;

            var doorCell = services.FindHorizontalDoorCell(doorEntity);

            if (doorCell != doorEntity)
            {
                //Logging: Info("Open from right!")
                var nextDoorCell = services.GetEntityByDataGrid(doorCell, 1, 0);
                OpenDoor(doorCell, nextDoorCell, "Horizontal");
            }
            else
            {
                if (services.IsSameCellType(doorCell, 1, 0))
                {
                    //Logging: Info("Open from left!")
                    var nextDoorCell = services.GetEntityByDataGrid(doorCell, 1, 0);
                    OpenDoor(doorCell, nextDoorCell, "Horizontal");
                }
                else
                {
                    //Single cell horizontally, might be vertical Door
                    doorCell = services.FindVerticalDoorCell(doorEntity);
                    var nextDoorCell = services.GetEntityByDataGrid(doorCell, 0, 1);
                    OpenDoor(doorCell, nextDoorCell, "Vertical");
                }
            }

            void OpenDoor(IEntity doorCell, IEntity nextDoorCell, string flavor)
            {
                services.Logger.LogInformation("Opening {0} Door...", flavor);

                var mapEntity = services.Entities.GetMapEntity(doorCell.WorldId);

                var position = doorCell.Get<PositionComponent>().Value;
                var targetCell = mapEntity.GetTileGridCell(position);

                var findPattern = @$"Vanilla\/(?<level>\w+)\/{className}\/{flavor}\/\w+";

                if (services.TryGetCellGfxFlavor(targetCell, findPattern, out string level))
                {
                    var stampName = $"Vanilla/{level}/{className}/{flavor}/Opened";
                    var stampId = services.Stamps.GetByName(stampName).Id;
                    mapEntity.PutStampAtEntityPosition(doorCell, stampId, 0);
                }


                var clipName = $"Vanilla/{level}/{className}/Opening/{flavor}";
                var soundName = $"Vanilla/Common/{className}/Opening";

                var clipId = services.Clips.GetId(clipName);
                var soundId = services.Sounds.GetByName(soundName);

                doorCell.SetSpriteOn();
                doorCell.PlayAnimation(0, clipId);
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

                nextDoorCell.SetState<DoorState>(DoorState.Opening);
            }

            void RemoveHorizontalDoorObstacle(IEntity doorCell, AnimFinishedEvent e)
            {
                services.Logger.LogInformation("Removing Horizontal Door Obstacle...");

                doorCell.SetSpriteOff();
                doorCell.SetBodyOffEx();


                var nextDoorCell = services.GetEntityByDataGrid(doorCell, 1, 0);

                nextDoorCell.SetBodyOffEx();
                nextDoorCell.SetState<DoorState>(DoorState.Open);

                services.Entities.RequestErase(doorCell);
                services.Entities.RequestErase(nextDoorCell);
            }

            void RemoveVerticalDoorObstacle(IEntity doorCell, AnimFinishedEvent e)
            {
                services.Logger.LogInformation("Removing Vertical Door Obstacle...");

                doorCell.SetSpriteOff();
                doorCell.SetBodyOffEx();

                var nextDoorCell = services.GetEntityByDataGrid(doorCell, 0, 1);
                nextDoorCell.SetBodyOffEx();

                nextDoorCell.SetState<DoorState>(DoorState.Open);

                services.Entities.RequestErase(doorCell);
                services.Entities.RequestErase(nextDoorCell);
            }
        }

        private void CheckLock(IEntity context, TouchImpulse impulse, DoorState from, DoorState to)
        {
            var doorEntity = context;

            var metaData = doorEntity.GetMetadata();

            var actorEntity = impulse.Actor;

            services.Logger.LogInformation("Door Open!");

            if(doorEntity.TryGetMetadata("RequiredKey", out var option) && !string.IsNullOrEmpty(option))
            {
                var keycardItemId = services.Items.GetItemId(option);

                if (keycardItemId != -1)
                {
                    services.Logger.LogInformation("KeyCard {0} required.", option);

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

            machine.Process(doorEntity, new OpenDoorImpulse(actorEntity));
        }

        private void DoorLockedToClosed(IEntity context, UnlockImpulse impulse, DoorState from, DoorState to)
        {
            //throw new NotImplementedException();
        }

        private void DoorLockedToLocked(IEntity context, UseImpulse impulse, DoorState from, DoorState to)
        {
        }

        private void DoorLockedToLocked(IEntity context, TouchImpulse impulse, DoorState from, DoorState to)
        {
            throw new NotImplementedException();
        }

        #endregion Private Methods
    }
}
using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Abstractions;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Systems.Door
{
    public enum DoorState
    {
        Closed,
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

    public record TouchImpulse(IEntity Actor) : IFsmImpulse;
    public record UseImpulse(IEntity Actor) : IFsmImpulse;
    public record UnlockImpulse(IEntity Actor) : IFsmImpulse;

    internal class DoorSystem : IOnEntityCollisionSystem
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly IEventsMan eventsMan;
        private readonly IFsmMachine<IEntity, DoorState> machine;

        #endregion Private Fields

        #region Public Constructors

        public DoorSystem(IGameServices services, IEventsMan eventsMan, IFsmMachineFactory<IEntity> fsmMachineFactory)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.machine = fsmMachineFactory.GetMachine<DoorState>()
                .Register<TouchImpulse>(DoorState.Locked, Unlocking)
                .Register<TouchImpulse>(DoorState.Closed, ClosedToOpening)
                .Register<CompletionImpulse>(DoorState.Opening, OpeningToOpen);
        }

        #endregion Public Constructors

        #region Public Properties

        public int ColliderTypeA => ColliderTypes.ActorBody;

        public IEnumerable<int> ColliderTypesB
        {
            get
            {
                yield return ColliderTypes.OpenDoorTrigger;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnCollision(IFixture actorFixture, IEntity actorEntity, IFixture obstacleFixture, IEntity doorEntity, float dt, Vector2 projection)
        {
            machine.Process(doorEntity, new TouchImpulse(actorEntity));
        }

        #endregion Public Methods

        #region Private Methods

        private DoorState OpeningToOpen(IEntity context, CompletionImpulse impulse, DoorState from)
        {
            var doorEntity = context;
            var mapEntity = services.Entities.GetMapEntity(doorEntity.WorldId);

            services.Logger.LogInformation("Removing Door Obstacle...");

            doorEntity.SetSpriteOff();
            doorEntity.SetBodyOffEx();

            var className = services.Classes.GetById(doorEntity.ClassId).Name;
            var flavor = doorEntity.GetMetadata("Flavor");
            var level = doorEntity.GetMetadata("Level");
            var stampName = $"{level}/{className}/{flavor}/Opened";
            var stampId = services.Stamps.GetByName(stampName).Id;
            mapEntity.PutStampAtEntityPosition(doorEntity, stampId, 0);

            services.Entities.RequestErase(doorEntity);
            return DoorState.Open;
        }

        private DoorState ClosedToOpening(IEntity context, TouchImpulse impulse, DoorState from)
        {
            var doorEntity = context;

            var metaData = doorEntity.GetMetadata();
            var className = services.Classes.GetById(doorEntity.ClassId).Name;
            var actorEntity = impulse.Actor;
            var flavor = doorEntity.GetMetadata("Flavor");
            var level = doorEntity.GetMetadata("Level");

            services.Logger.LogInformation("Opening Door...");

            var clipName = $"{level}/{className}/Opening/{flavor}";
            var soundName = $"Vanilla/Common/{className}/Opening";

            var clipId = services.Clips.GetId(clipName);
            var soundId = services.Sounds.GetByName(soundName);

            doorEntity.SetSpriteOn();
            doorEntity.EmitSound(soundId);

            services.PlayAnimation(doorEntity,
                clipId,
                (e) => machine.Complete(doorEntity));

            return DoorState.Opening;
        }

        private DoorState Unlocking(IEntity context, TouchImpulse impulse, DoorState from)
        {
            var doorEntity = context;

            var metaData = doorEntity.GetMetadata();

            var actorEntity = impulse.Actor;

            if (doorEntity.TryGetMetadata("RequiredKey", out var requiredKey) && !string.IsNullOrEmpty(requiredKey))
            {
                var keycardItemId = services.Items.GetItemId(requiredKey);

                if (keycardItemId == -1)
                {
                    services.Logger.LogError($"Unknown key item '{requiredKey}' required.");
                    return DoorState.Locked;
                }

                services.Logger.LogInformation("KeyCard {0} required.", requiredKey);

                var actorInventory = actorEntity.GetInventory();

                var itemSlot = actorInventory.GetItemSlot(keycardItemId);

                //No keycard item then door can't be opened
                if (itemSlot is null)
                {
                    services.Logger.LogInformation("No KeyCard!");
                    return DoorState.Locked;
                }
            }

            return DoorState.Closed;
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
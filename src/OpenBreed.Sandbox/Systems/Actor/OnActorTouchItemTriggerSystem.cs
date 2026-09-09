using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Abstractions;
using OpenBreed.Animation.Generic;
using OpenBreed.Audio.Abstractions;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenBreed.Wecs.Physics.Systems.Helpers;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class OnActorTouchItemTriggerSystem : IOnActorTouchObstacleSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public OnActorTouchItemTriggerSystem(
            IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "ActorTouch";
        public string ActionName => "Pickup";

        #endregion Public Properties

        #region Public Methods

        public void OnTouch(
            IFixture actorFixture, IEntity actorEntity,
            IFixture triggerFixture, IEntity triggerEntity,
            Vector2 projection)
        {
            var itemEntity = triggerEntity;

            var mapEntity = services.Entities.GetMapEntity(itemEntity.WorldId);
            var metaData = itemEntity.GetMetadata();
            var pickupClassName = services.Classes.GetById(itemEntity.ClassId).Name;

            var pickupName = pickupClassName;

            if (!HandlePickup(actorEntity, itemEntity, pickupName))
            {
                services.Logger.LogError("Unknown pickup with name '{0}'.", pickupName);
                return;
            }

            var flavor = itemEntity.GetMetadata("Flavor");

            var position = itemEntity.Get<PositionComponent>().Value;
            var targetCell = mapEntity.GetTileGridCell(position);
            var stampName = $"{flavor}/Picked";
            var stampId = services.Stamps.GetByName(stampName).Id;
            mapEntity.PutStampAtEntityPosition(itemEntity, stampId, 0);

            var soundName = $"Vanilla/Common/{pickupClassName}/Picked";
            var soundId = services.Sounds.GetByName(soundName);

            itemEntity.EmitSound(soundId);

            services.Worlds.RequestRemoveEntity(itemEntity);
            services.Entities.RequestErase(itemEntity);
        }

        #endregion Public Methods

        #region Private Methods

        private bool TryGiveItem(IEntity actorEntity, string itemName, int quantity)
        {
            if (!services.Items.TryGetItemId(itemName, out var itemId))
            {
                return false;
            }

            actorEntity.GiveItem(itemId, quantity);
            services.Logger.LogInformation("Picked up {0} '{1}'.", quantity, itemName);
            return true;
        }

        private bool HandleItemPickup(IEntity actorEntity, IEntity itemEntity, string itemClassName)
        {
            switch (itemClassName)
            {
                case "Credits":
                    if (itemEntity.TryGetMetadata<int>("Value", out var value))
                    {
                        return TryGiveItem(actorEntity, itemClassName, value);
                    }

                    return TryGiveItem(actorEntity, itemClassName, 100);

                case "Keycard":
                    if (itemEntity.TryGetMetadata("KeyId", out var keyId))
                    {
                        return TryGiveItem(actorEntity, $"Keycard{keyId}", 1);
                    }

                    return TryGiveItem(actorEntity, itemClassName, 1);

                default:
                    return TryGiveItem(actorEntity, itemClassName, 1);
            }
        }

        private bool HandlePickup(IEntity actorEntity, IEntity itemEntity, string pickupName)
        {
            switch (pickupName)
            {
                case "ExtraLife":
                    actorEntity.AddLives(1);
                    services.Logger.LogInformation("Picked up '{0}'.", pickupName);
                    return true;

                case "MedkitSmall":
                    //TODO: Implement that
                    services.Logger.LogInformation("Picked up '{0}'.", pickupName);
                    return true;

                case "MedkitBig":
                    //TODO: Implement that
                    services.Logger.LogInformation("Picked up '{0}'.", pickupName);
                    return true;

                default:
                    return HandleItemPickup(actorEntity, itemEntity, pickupName);
            }
        }

        #endregion Private Methods
    }
}
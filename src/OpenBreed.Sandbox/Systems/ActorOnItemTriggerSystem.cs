using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Actor;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Physics.Abstractions;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Systems
{
    public class ActorOnItemTriggerSystem : IActorOnTriggerSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public ActorOnItemTriggerSystem(
            IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string ActionName => "Vanilla/Common/Pickables/Item";

        #endregion Public Properties

        #region Public Methods

        public void OnTrigger(IEntity actorEntity, IEntity triggerEntity)
        {
            var itemEntity = triggerEntity;

            var mapEntity = services.Entities.GetMapEntity(itemEntity.WorldId);
            var metaData = itemEntity.GetMetadata();
            var name = metaData.Name;

            services.Logger.LogInformation("ItemEntityId: {0}", itemEntity.Id);
            services.Logger.LogInformation("ActorEntityId: {0}", actorEntity.Id);

            if (!HandlePickup(actorEntity, name))
            {
                services.Logger.LogError("Unknown item with name '{0}'.", name);
                return;
            }

            var stampName = $"{metaData.Level}/{name}/{metaData.Flavor}/Picked";
            var soundName = $"Vanilla/Common/{name}/Picked";

            var stampId = services.Stamps.GetByName(stampName).Id;
            var soundId = services.Sounds.GetByName(soundName);

            mapEntity.PutStampAtEntityPosition(itemEntity, stampId, 0);

            itemEntity.EmitSound(soundId);

            services.Worlds.RequestRemoveEntity(itemEntity);
            services.Entities.RequestErase(itemEntity);
        }

        #endregion Public Methods

        #region Private Methods

        private bool HandleItem(IEntity actorEntity, string itemName)
        {
            if (!services.Items.TryGetItemId(itemName, out var itemId))
            {
                return false;
            }

            switch (itemName)
            {
                case "CreditsSmall":
                    actorEntity.GiveItem(itemId, 100);
                    services.Logger.LogInformation("Picked up '{0}' credits.", 100);
                    return true;

                case "CreditsBig":
                    actorEntity.GiveItem(itemId, 1000);
                    services.Logger.LogInformation("Picked up '{0}' credits.", 1000);
                    return true;

                default:
                    actorEntity.GiveItem(itemId, 1);
                    services.Logger.LogInformation("Picked up '{0}'.", itemName);
                    return true;
            }
        }

        private bool HandlePickup(IEntity actorEntity, string pickupName)
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
                    return HandleItem(actorEntity, pickupName);
            }
        }

        #endregion Private Methods
    }
}
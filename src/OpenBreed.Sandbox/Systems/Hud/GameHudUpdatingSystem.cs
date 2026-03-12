using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Rendering.Systems.Events;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace OpenBreed.Common.Game.Wecs.Systems.Hud
{
    public class GameHudUpdatingSystem :
        IEventSystem<InventoryChangedEvent>,
        IEventSystem<LivesChangedEvent>,
        IEventSystem<EntityEnteredEvent>,
        IEventSystem<DamagedEvent>
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public GameHudUpdatingSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(EntityEnteredEvent e)
        {
        }

        public void Update(DamagedEvent e)
        {
            var actorEntity = services.Entities.GetById(e.EntityId);

            var healthComponent = actorEntity.Get<HealthComponent>();
            var percent = healthComponent.GetPercent();
            TryUpdateBar("Hud/HealthBar/P1", 64, percent);
        }

        public void Update(InventoryChangedEvent e)
        {
            var actorEntity = services.Entities.GetById(e.EntityId);

            if (e.ItemId == ItemTypes.KeycardStandard)
            {
                var inventoryComponent = actorEntity.Get<InventoryComponent>();

                var slot = inventoryComponent.GetItemSlot(ItemTypes.KeycardStandard);
                var quantity = slot.GetItemQuantity(ItemTypes.KeycardStandard);

                TryUpdateHudText("Hud/KeysCounter/P1", quantity.ToString().PadLeft(2, '0'));

                return;
            }

            if (e.ItemId == ItemTypes.Ammo)
            {
                var inventoryComponent = actorEntity.Get<InventoryComponent>();
                var ammoComponent = actorEntity.Get<AmmoComponent>();
                var slot = inventoryComponent.GetItemSlot(ItemTypes.Ammo);
                var quantity = slot.GetItemQuantity(ItemTypes.Ammo);
                var percent = ammoComponent.GetRoundsPercent();

                var text = quantity > 9 ? "+" : quantity.ToString();

                TryUpdateHudText("Hud/AmmoCounter/P1", text);
                TryUpdateBar("Hud/AmmoBar/P1", 32, percent);

                return;
            }
        }

        public void Update(LivesChangedEvent e)
        {
            var actorEntity = services.Entities.GetById(e.EntityId);

            var livesComponent = actorEntity.Get<LivesComponent>();

            var livesCount = livesComponent.Value;

            TryUpdateHudText("Hud/LivesCounter/P1", livesCount.ToString().PadLeft(2, '0'));
        }

        #endregion Public Methods

        #region Private Methods

        private void TryUpdateHudText(string hudElementTag, string text)
        {
            var hudElementEntity = services.Entities.GetByTag(hudElementTag).FirstOrDefault();

            if (hudElementEntity is null)
            {
                return;
            }

            hudElementEntity.SetText(0, text);
        }

        private void TryUpdateBar(string hudElementTag, int barSize, float percent)
        {
            var hudElementEntity = services.Entities.GetByTag(hudElementTag).FirstOrDefault();

            if (hudElementEntity is null)
            {
                return;
            }

            var spriteComponent = hudElementEntity.Get<SpriteComponent>();
            spriteComponent.Scale = new Vector2((int)(barSize * percent), 1);
        }

        #endregion Private Methods
    }
}
using Microsoft.Extensions.Logging;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Core.Systems;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(InventoryComponent))]
    public class ItemManagingSystem : UpdatableMatchingSystemBase
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;
        private readonly ItemsMan itemsMan;

        #endregion Private Fields

        #region Public Constructors

        public ItemManagingSystem(
            IWorldMan worldMan,
            ItemsMan itemsMan,
            IEventsMan eventsMan,
            ILogger logger) : base(worldMan)
        {
            this.itemsMan = itemsMan;
            this.eventsMan = eventsMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var inventoryComponent = entity.Get<InventoryComponent>();

            var toAdd = inventoryComponent.ToAdd;

            for (int i = 0; i < toAdd.Count; i++)
            {
                AddItem(entity, toAdd[i].Item1, toAdd[i].Item2);
            }

            toAdd.Clear();
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddItem(IEntity entity, int itemId, int quantity = 1)
        {
            var inventoryCmp = entity.Get<InventoryComponent>();

            var itemSlot = inventoryCmp.GetItemSlot(itemId);

            if (itemSlot is null)
                itemSlot = inventoryCmp.GetFirstEmptySlot();

            itemSlot.AddItem(itemId, quantity);

            eventsMan.Raise(new InventoryChangedEvent(entity.Id, itemId, quantity));
        }

        #endregion Private Methods
    }
}
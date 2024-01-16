using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Sandbox.Wecs.Events;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    [RequireEntityWith(
        typeof(InventoryComponent))]
    public class ItemManagingSystem : UpdatableSystemBase<ItemManagingSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;
        private readonly ItemsMan itemsMan;

        #endregion Private Fields

        #region Public Constructors

        public ItemManagingSystem(
            ItemsMan itemsMan,
            IEventsMan eventsMan,
            ILogger logger)
        {
            this.itemsMan = itemsMan;
            this.eventsMan = eventsMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
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

            eventsMan.Raise(entity, new InventoryChangedEvent(entity.Id, itemId, quantity));
        }

        #endregion Private Methods
    }
}
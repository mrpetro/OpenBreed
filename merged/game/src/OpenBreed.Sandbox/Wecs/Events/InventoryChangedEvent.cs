using OpenBreed.Wecs.Events;

namespace OpenBreed.Sandbox.Wecs.Events
{
    public class InventoryChangedEvent : EntityEvent
    {
        #region Public Constructors

        public InventoryChangedEvent(int entityId, int itemId, int quantity)
            : base(entityId)
        {
            ItemId = itemId;
            Quantity = quantity;
        }

        #endregion Public Constructors

        #region Public Properties

        public int ItemId { get; }
        public int Quantity { get; }

        #endregion Public Properties
    }
}
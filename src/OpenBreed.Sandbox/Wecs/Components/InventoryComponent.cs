using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public class InventorySlot
    {
        #region Private Fields

        private readonly Dictionary<int, int> items = new Dictionary<int, int>();

        #endregion Private Fields

        #region Public Constructors

        public InventorySlot(string name)
        {
            Name = name;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Name of this bag. I.e. "Backpack"
        /// </summary>
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public bool ContainsItem(int itemId) => items.ContainsKey(itemId);

        public bool IsEmpty => items.Count == 0;

        public void AddItem(int itemId, int quantity = 1)
        {
            if (items.TryGetValue(itemId, out int currentQuantity))
                currentQuantity += quantity;
            else
                currentQuantity = quantity;

            items[itemId] = currentQuantity;
        }

        #endregion Public Methods
    }

    public class InventoryComponent : IEntityComponent
    {
        #region Public Constructors

        public InventoryComponent(int slotCount)
        {
            Slots = new InventorySlot[slotCount];

            for (int i = 0; i < Slots.Length; i++)
                Slots[i] = new InventorySlot(i.ToString());
        }

        public InventoryComponent(InventorySlot[] slots)
        {
            Slots = slots;
        }

        #endregion Public Constructors

        #region Public Properties

        public InventorySlot[] Slots { get; }

        #endregion Public Properties

        #region Public Methods

        public InventorySlot GetItemSlot(int itemId)
        {
            return Slots.FirstOrDefault(slot => slot.ContainsItem(itemId));
        }

        public InventorySlot GetFirstEmptySlot()
        {
            return Slots.FirstOrDefault(slot => slot.IsEmpty);
        }

        #endregion Public Methods
    }
}
using OpenBreed.Core.Common.Systems.Components;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Components
{
    public class Bag
    {
        #region Private Fields

        private readonly Dictionary<int, int> Items = new Dictionary<int, int>();

        #endregion Private Fields

        #region Public Methods

        public void AddItem(int itemId, int quantity = 1)
        {
            if (Items.TryGetValue(itemId, out int currentQuantity))
                currentQuantity += quantity;
            else
                currentQuantity = quantity;

            Items[itemId] = currentQuantity;
        }

        #endregion Public Methods
    }

    public class InventoryComponent : IEntityComponent
    {
        #region Public Constructors

        public InventoryComponent(Bag[] bags)
        {
            Bags = bags;
        }

        #endregion Public Constructors

        #region Public Properties

        public Bag[] Bags { get; }

        #endregion Public Properties
    }
}
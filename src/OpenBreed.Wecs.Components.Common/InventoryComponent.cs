using OpenBreed.Wecs.Components;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Common
{
    public class Bag
    {
        #region Private Fields

        private readonly Dictionary<int, int> Items = new Dictionary<int, int>();

        #endregion Private Fields

        #region Public Constructors

        public Bag(string name)
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
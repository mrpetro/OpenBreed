using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Managers
{
    public class ItemsMan
    {
        #region Private Fields

        private readonly Dictionary<int, string> itemTypes = new Dictionary<int, string>();
        private readonly Dictionary<string, int> itemNames = new Dictionary<string, int>();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public ItemsMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public int RegisterItemType(string name)
        {
            var newItemId = itemTypes.Count;
            itemTypes.Add(newItemId, name);
            itemNames.Add(name, newItemId);
            return newItemId;
        }

        public string GetItemName(int itemId)
        {
            return itemTypes[itemId];
        }

        public int GetItemId(string itemName)
        {
            if (itemNames.TryGetValue(itemName, out int itemId))
                return itemId;
            else
                return -1;
        }

        #endregion Public Methods
    }
}
using OpenBreed.Common.Logging;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Managers
{
    public class ItemsMan
    {
        #region Private Fields

        private readonly Dictionary<int, string> itemTypes = new Dictionary<int, string>();
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
            itemTypes.Add(itemTypes.Count, name);
            return itemTypes.Count - 1;
        }

        public string GetItemName(int itemId)
        {
            return itemTypes[itemId];
        }

        #endregion Public Methods
    }
}
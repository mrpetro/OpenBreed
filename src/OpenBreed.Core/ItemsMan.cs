using OpenBreed.Core.Collections;
using System;

namespace OpenBreed.Core
{
    public class ItemsMan
    {
        #region Private Fields

        private readonly IdMap<Item> ids = new IdMap<Item>();

        #endregion Private Fields

        #region Public Constructors

        public ItemsMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void Register(Item item)
        {
            item.Id = ids.Add(item);
        }

        #endregion Public Methods
    }
}
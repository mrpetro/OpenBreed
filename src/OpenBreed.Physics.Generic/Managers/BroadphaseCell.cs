using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class BroadphaseCell
    {
        #region Private Fields

        private readonly List<int> items = new List<int>();

        #endregion Private Fields

        #region Internal Methods

        internal void AddItem(int itemId)
        {
            items.Add(itemId);
        }

        internal void InsertTo(HashSet<int> result)
        {
            foreach (var item in items)
                result.Add(item);
        }

        internal void RemoveItem(int itemId)
        {
            items.Remove(itemId);
        }

        #endregion Internal Methods
    }
}
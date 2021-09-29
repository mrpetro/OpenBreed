using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public struct InventoryItem
    {
    }

    public class ItemComponent
    {
        public ItemComponent()
        {
            Items = new List<InventoryItem>();
        }

        public List<InventoryItem> Items { get; }
    }
}

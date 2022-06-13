using OpenBreed.Sandbox.Components;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class EntityExtensions
    {
        public static void GiveItem(this Entity entity, int itemId, int quantity = 1)
        {
            var inventoryCmp = entity.Get<InventoryComponent>();

            var itemSlot = inventoryCmp.GetItemSlot(itemId);

            if (itemSlot is null)
                itemSlot = inventoryCmp.GetFirstEmptySlot();

            itemSlot.AddItem(itemId, quantity);
        }

        public static Entity GetPlayerCamera(this Entity entity, IEntityMan entityMan)
        {
            return entity.Get<FollowedComponent>().FollowerIds.Select(item => entityMan.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "PlayerCamera");
        }

    }
}

using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Components;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenTK.Mathematics;
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

        public static void SetPosition(this Entity target,
                                       IEntityMan entityMan,
                                       IShapeMan shapeMan,
                                       Entity entryEntity)
        {
            var pairId = entryEntity.Tag.Split('/')[1];
            // Search for all exits from same world as entry with same pair ID 
            var exitEntity = entityMan.GetByTag($"TeleportExit/{pairId}").FirstOrDefault(item => item.WorldId == entryEntity.WorldId);

            if (exitEntity is null)
                throw new Exception("No exit entity found");

            var exitPos = exitEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();

            var bodyCmp = target.Get<BodyComponent>();
            var shape = shapeMan.GetById(bodyCmp.Fixtures.First().ShapeId);
            var targetAabb = shape.GetAabb().Translated(targetPos.Value);

            var offset = new Vector2((32 - targetAabb.Size.X) / 2.0f, (32 - targetAabb.Size.Y) / 2.0f);

            var newPosition = exitPos.Value + offset;

            targetPos.Value = newPosition;

            var velocityCmp = target.Get<VelocityComponent>();
            velocityCmp.Value = Vector2.Zero;

            var thrustCmp = target.Get<ThrustComponent>();
            thrustCmp.Value = Vector2.Zero;

            target.State = null;
        }
    }
}

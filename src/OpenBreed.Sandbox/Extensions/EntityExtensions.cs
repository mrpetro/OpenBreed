using OpenBreed.Core;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Components;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
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
        public static InventoryComponent GetInventory(this Entity entity)
        {
            return entity.Get<InventoryComponent>();
        }

        public static Entity GetEntityByDataGrid(this Entity entity, IWorldMan worldMan, int ox, int oy)
        {
            var pos = entity.Get<PositionComponent>();
            var world = worldMan.GetById(entity.WorldId);
            var dataGrid = world.GetModule<IDataGrid<Entity>>();
            var indexPos = new Vector2i((int)pos.Value.X / 16, (int)pos.Value.Y / 16);
            var thisEntity = dataGrid.Get(indexPos);
            var indexIndexPos = Vector2i.Add(indexPos, new Vector2i(ox, oy));
            var resultEntity = dataGrid.Get(indexIndexPos);

            return resultEntity;
        }

        public static bool IsSameCellType(this Entity entity, IWorldMan worldMan, int ox, int oy)
        {
            var nextCell = entity.GetEntityByDataGrid(worldMan, ox, oy);

            var nextCellMeta = nextCell.TryGet<MetadataComponent>();

            if (nextCellMeta is null)
                return false;

            var thisData = entity.Get<MetadataComponent>();

            return nextCellMeta.Name == thisData.Name && nextCellMeta.Option == thisData.Option;
        }

        public static Entity FindVerticalDoorCell(this Entity entity, IWorldMan worldMan)
        {
            var foundCell = entity;

            var thisData = entity.Get<MetadataComponent>();

            var nextCell = foundCell;

            while (nextCell is not null)
            {
                var nextCellMeta = nextCell.TryGet<MetadataComponent>();

                if (nextCellMeta is null)
                    break;

                if (!(nextCellMeta.Name == thisData.Name && nextCellMeta.Option == thisData.Option))
                    break;

                foundCell = nextCell;

                nextCell = nextCell.GetEntityByDataGrid(worldMan, 0, -1);
            }

            return foundCell;
        }

        public static Entity FindHorizontalDoorCell(this Entity entity, IWorldMan worldMan)
        {
            var foundCell = entity;

            var thisData = entity.Get<MetadataComponent>();

            var nextCell = foundCell;

            while (nextCell is not null)
            {
                var nextCellMeta = nextCell.TryGet<MetadataComponent>();

                if (nextCellMeta is null)
                    break;

                if (!(nextCellMeta.Name == thisData.Name && nextCellMeta.Option == thisData.Option))
                    break;

                foundCell = nextCell;

                nextCell = nextCell.GetEntityByDataGrid(worldMan, -1, 0);
            }

            return foundCell;
        }

        public static void SetBodyOffEx(this Entity entity)
        {
            var bodyCmp = entity.Get<BodyComponent>();
            var fixture = bodyCmp.Fixtures.First();
            fixture.GroupIds.RemoveAll(id => id == ColliderTypes.FullObstacle);
        }

        public static void GiveItem(this Entity entity, int itemId, int quantity = 1)
        {
            var inventoryCmp = entity.Get<InventoryComponent>();

            var itemSlot = inventoryCmp.GetItemSlot(itemId);

            if (itemSlot is null)
                itemSlot = inventoryCmp.GetFirstEmptySlot();

            itemSlot.AddItem(itemId, quantity);
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
            var shape = bodyCmp.Fixtures.First().Shape;
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

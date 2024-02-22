using OpenBreed.Core;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class EntityExtensions
    {
        public static InventoryComponent GetInventory(this IEntity entity)
        {
            return entity.Get<InventoryComponent>();
        }

        public static bool HasHealth(this IEntity entity)
        {
            return entity.Contains<HealthComponent>();
        }

        public static void RestoreFillHealth(this IEntity entity)
        {
            var cmp = entity.Get<HealthComponent>();
            cmp.Value = cmp.MaximumValue;
        }

        public static HealthComponent GetHealth(this IEntity entity)
        {
            return entity.Get<HealthComponent>();
        }

        public static int GetLives(this IEntity entity)
        {
            return entity.Get<LivesComponent>().Value;
        }

        public static void AddLives(this IEntity entity, int value)
        {
            entity.Get<LivesComponent>().ToAdd.Add(value);
        }

        public static void SetResurrectable(this IEntity entity, int worldId)
        {
            entity.Set<ResurrectableComponent>(new ResurrectableComponent(worldId));
        }

        public static void Resurrect(this IEntity entity)
        {
            entity.Set<ResurrectCommandComponent>(new ResurrectCommandComponent());
        }

        public static void InflictDamage(this IEntity entity, int amount, int targetEntityId)
        {
            var damageComponent = entity.Get<DamagerComponent>();
            damageComponent.Inflictions.Add(new DamageInfliction(amount, new[] { targetEntityId }));
        }

        public static void CreateSlowdown(this IEntityFactory entityFactory, IWorldMan worldMan, IEntity entity, int worldId, int ox, int oy)
        {
            var indexPos = entity.GetIndexPos(ox, oy);

            var toCreate = entityFactory.Create(@"ABTA\Templates\Common\Environment\SlowdownObstacle")
                .SetParameter("startX", 16 * indexPos.X)
                .SetParameter("startY", 16 * indexPos.Y)
                .Build();

            worldMan.RequestAddEntity(toCreate, worldId);
        }

        public static Vector2i GetIndexPos(this IEntity entity, int ox, int oy)
        {
            var pos = entity.Get<PositionComponent>();
            var indexPos = new Vector2i((int)pos.Value.X / 16, (int)pos.Value.Y / 16);
            return Vector2i.Add(indexPos, new Vector2i(ox, oy));
        }

        public static void SetBodyOffEx(this IEntity entity)
        {
            var bodyCmp = entity.Get<BodyComponent>();
            var fixture = bodyCmp.Fixtures.First();
            fixture.GroupIds.RemoveAll(id => id == ColliderTypes.FullObstacle);
        }

        public static void GiveItem(this IEntity entity, int itemId, int quantity = 1)
        {
            var inventoryCmp = entity.Get<InventoryComponent>();
            inventoryCmp.ToAdd.Add((itemId, quantity));
        }

        public static void SetPositionToExit(this IEntity target,
                                       IEntityMan entityMan,
                                       IShapeMan shapeMan,
                                       IEntity entryEntity)
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

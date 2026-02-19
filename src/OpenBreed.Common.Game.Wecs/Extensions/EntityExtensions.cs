using OpenBreed.Core;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;

namespace OpenBreed.Common.Game.Wecs.Extensions
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

        public static void RestoreFullHealth(this IEntity entity)
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
            entity.Set(new ResurrectableComponent(worldId));
        }

        public static void Resurrect(this IEntity entity)
        {
            entity.Set(new ResurrectCommandComponent());
        }

        public static int GetTrackedEntityId(this IEntity entity)
        {
            return entity.Get<TrackingComponent>().EntityId;
        }

        public static int NextWeapon(this IEntity entity)
        {
            var weapons = entity.Get<WeaponsComponent>();

            var currentWeaponNo = weapons.CurrentWeaponNo;

            currentWeaponNo++;

            if (currentWeaponNo > 4)
            {
                currentWeaponNo = 1;
            }

            weapons.CurrentWeaponNo = currentWeaponNo;
            weapons.CurrentWeaponState = 0;
            return currentWeaponNo;
        }

        public static bool HasTrackedEntity(this IEntity entity)
        {
            return entity.Get<TrackingComponent>().EntityId != -1;
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

    }
}

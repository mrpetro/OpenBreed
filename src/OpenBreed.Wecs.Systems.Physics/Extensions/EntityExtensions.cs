using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Extensions;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Extensions
{
    public static class EntityExtensions
    {
        public static Box2 GetAabb(this IEntity entity)
        {
            var pos = entity.Get<PositionComponent>();
            var body = entity.Get<BodyComponent>();

            var shape = body.Fixtures.First().Shape;
            return shape.GetAabb().Translated(pos.Value);
        }

        public static void RemoveEntityFromStatics(this IEntity entity, IEntity bodyEntity)
        {
            var grid = entity.Get<CollisionComponent>().Grid;
            grid.RemoveStatic(bodyEntity.Id);
        }

        public static void UpdateDynamics(this IEntity entity, IEntityMan entityMan)
        {
            var dynamic = entity.Get<CollisionComponent>().Dynamic;
            dynamic.UpdateItems((itemId) => CalculateAabb(entityMan.GetById(itemId)));
        }

        private static Box2 CalculateAabb(IEntity entity)
        {
            var body = entity.Get<BodyComponent>();
            var pos = entity.Get<PositionComponent>();
            var shape = body.Fixtures.First().Shape;
            return shape.GetAabb().Translated(pos.Value);
        }


        public static void AddEntityToDynamics(this IEntity entity, IEntity bodyEntity)
        {
            var aabb = CalculateAabb(bodyEntity);

            var dynamic = entity.Get<CollisionComponent>().Dynamic;
            dynamic.InsertItem(bodyEntity.Id, aabb);

        }

        public static void RemoveEntityFromDynamic(this IEntity entity, IEntity bodyEntity)
        {
            var dynamic = entity.Get<CollisionComponent>().Dynamic;
            dynamic.RemoveItem(bodyEntity.Id);
        }

        public static void AddEntityToStatics(this IEntity entity, IEntity bodyEntity )
        {
            var grid = entity.Get<CollisionComponent>().Grid;
            var pos = bodyEntity.Get<PositionComponent>();
            var body = bodyEntity.Get<BodyComponent>();


            var aabb = body.Fixtures[0].Shape.GetAabb();

            for (int i = 1; i < body.Fixtures.Count; i++)
            {
                aabb = aabb.Inflated(body.Fixtures[i].Shape.GetAabb());
            }

            aabb = aabb.Translated(pos.Value);

            grid.InsertItem(bodyEntity.Id, aabb);
        }
    }
}

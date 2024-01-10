using OpenBreed.Physics.Interface;
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
            var grid = entity.Get<BroadphaseStaticComponent>().Grid;
            grid.RemoveStatic(bodyEntity.Id);
        }

        public static void UpdateDynamics(this IEntity entity, IEntityMan entityMan)
        {
            var dynamic = entity.Get<BroadphaseDynamicComponent>().Dynamic;
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

            var dynamic = entity.Get<BroadphaseDynamicComponent>().Dynamic;
            dynamic.InsertItem(bodyEntity.Id, aabb);

        }

        public static void RemoveEntityFromDynamic(this IEntity entity, IEntity bodyEntity)
        {
            var dynamic = entity.Get<BroadphaseDynamicComponent>().Dynamic;
            dynamic.RemoveItem(bodyEntity.Id);
        }

        public static void AddEntityToStatics(this IEntity entity, IEntity bodyEntity )
        {
            var grid = entity.Get<BroadphaseStaticComponent>().Grid;
            var pos = bodyEntity.Get<PositionComponent>();
            var body = bodyEntity.Get<BodyComponent>();

            var shape = body.Fixtures.First().Shape;
            var aabb = shape.GetAabb().Translated(pos.Value);

            grid.InsertItem(bodyEntity.Id, aabb);
        }

        public static void SetBodyOff(this IEntity entity)
        {
            var bodyCmp = entity.Get<BodyComponent>();

            if (bodyCmp.Inactive)
                return;

            bodyCmp.Inactive = true;
            entity.RaiseEvent(new BodyOffEventArgs(entity));
        }

        public static void SetBodyOn(this IEntity entity)
        {
            var bodyCmp = entity.Get<BodyComponent>();

            if (!bodyCmp.Inactive)
                return;

            bodyCmp.Inactive = false;
            entity.RaiseEvent(new BodyOffEventArgs(entity));
        }
    }
}

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

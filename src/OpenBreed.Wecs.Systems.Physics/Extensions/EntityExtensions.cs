using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Extensions
{
    public static class EntityExtensions
    {
        public static void SetBodyOff(this Entity entity)
        {
            var bodyCmp = entity.Get<BodyComponent>();

            if (bodyCmp.Inactive)
                return;

            bodyCmp.Inactive = true;
            entity.RaiseEvent(new BodyOffEventArgs(entity));
        }

        public static void SetBodyOn(this Entity entity)
        {
            var bodyCmp = entity.Get<BodyComponent>();

            if (!bodyCmp.Inactive)
                return;

            bodyCmp.Inactive = false;
            entity.RaiseEvent(new BodyOffEventArgs(entity));
        }
    }
}

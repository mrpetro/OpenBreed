using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Common.Extensions
{
    public static class EntityExtensions
    {
        public static void AddFollower(this Entity entity, Entity followerEntity)
        {
            var fc = entity.Get<FollowerComponent>();
            fc.FollowerIds.Add(followerEntity.Id);
        }
    }
}

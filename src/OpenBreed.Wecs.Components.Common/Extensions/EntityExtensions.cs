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
            var fc = entity.Get<FollowedComponent>();
            fc.FollowerIds.Add(followerEntity.Id);
        }

        public static void PauseWorld(this Entity entity)
        {
            entity.Set(new PauserComponent(pause: true));
        }

        public static void UnpauseWorld(this Entity entity)
        {
            entity.Set(new PauserComponent(pause: false));
        }

        public static MetadataComponent GetMetadata(this Entity entity)
        {
            return entity.Get<MetadataComponent>();
        }

        public static FollowedComponent GetFollowers(this Entity entity)
        {
            return entity.Get<FollowedComponent>();
        }

        public static PositionComponent GetPosition(this Entity entity)
        {
            return entity.Get<PositionComponent>();
        }

        public static void SetPosition(this Entity entity, float x, float y)
        {
            entity.Get<PositionComponent>().Value = new OpenTK.Mathematics.Vector2(x, y);
        }
    }
}

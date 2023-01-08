using OpenBreed.Wecs.Entities;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Common.Extensions
{
    public static class EntityExtensions
    {
        public static void AddFollower(this IEntity entity, IEntity followerEntity)
        {
            var fc = entity.Get<FollowedComponent>();
            fc.FollowerIds.Add(followerEntity.Id);
        }

        public static void PauseWorld(this IEntity entity)
        {
            entity.Set(new PauserComponent(pause: true));
        }

        public static void UnpauseWorld(this IEntity entity)
        {
            entity.Set(new PauserComponent(pause: false));
        }

        public static MetadataComponent GetMetadata(this IEntity entity)
        {
            return entity.Get<MetadataComponent>();
        }

        public static FollowedComponent GetFollowers(this IEntity entity)
        {
            return entity.Get<FollowedComponent>();
        }

        public static Vector2 GetPosition(this IEntity entity)
        {
            return entity.Get<PositionComponent>().Value;
        }

        public static void SetPosition(this IEntity entity, float x, float y)
        {
            entity.Get<PositionComponent>().Value = new OpenTK.Mathematics.Vector2(x, y);
        }
    }
}

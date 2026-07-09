using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Core.Components.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

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

        public static Vector2 GetVelocity(this IEntity entity)
        {
            return entity.Get<VelocityComponent>().Value;
        }

        public static bool IsMoving(this IEntity entity)
        {
            return entity.Get<VelocityComponent>().Value != Vector2.Zero;
        }

        public static void SetVelocity(this IEntity entity, float x, float y)
        {
            entity.Get<VelocityComponent>().Value = new OpenTK.Mathematics.Vector2(x, y);
        }

        public static Vector2 GetTargetDirection(this IEntity entity)
        {
            return entity.Get<AngularVelocityComponent>().Value;
        }

        public static void SetTargetDirection(this IEntity entity, float x, float y)
        {
            entity.Get<AngularVelocityComponent>().Value = new OpenTK.Mathematics.Vector2(x, y);
        }

        public static void SetTargetDirectionToCoordinates(this IEntity entity, Vector2 coordinates)
        {
            var direction = Vector2.Subtract(coordinates, entity.GetPosition());
            entity.Get<AngularVelocityComponent>().Value = direction.Normalized();
        }

        public static Vector2 GetDirection(this IEntity entity)
        {
            return entity.Get<AngularPositionComponent>().Value;
        }

        public static void SetDirection(this IEntity entity, float x, float y)
        {
            entity.Get<AngularPositionComponent>().Value = new OpenTK.Mathematics.Vector2(x, y);
        }

        public static Vector2 GetThrust(this IEntity entity)
        {
            return entity.Get<ThrustComponent>().Value;
        }

        public static void SetThrust(this IEntity entity, float x, float y)
        {
            entity.Get<ThrustComponent>().Value = new OpenTK.Mathematics.Vector2(x, y);
        }

        public static void SetMetadata<TValue>(this IEntity entity, string name, TValue value) where TValue : struct
        {
            entity.Get<MetadataComponent>().Attributes[name] = value;
        }

        public static void SetMetadata(this IEntity entity, string name, string value)
        {
            entity.Get<MetadataComponent>().Attributes[name] = value;
        }

        public static bool TryGetMetadata(this IEntity entity, string name, out string value)
            => TryGetMetadataPrivate<string>(entity, name, out value);

        public static bool TryGetMetadata<TValue>(this IEntity entity, string name, out TValue value) where TValue : struct
            => TryGetMetadataPrivate<TValue>(entity, name, out value);

        public static IEnumerable<string> GetActionsOnTrigger(this IEntity entity, string triggerName)
        {
            var sc = entity.TryGet<OnTriggerComponent>();

            if (sc is null)
            {
                yield break;
            }

            foreach (var action in sc.Actions.Where(item => item.Trigger == triggerName))
            {
                yield return action.Action;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static bool TryGetMetadataPrivate<TValue>(this IEntity entity, string name, out TValue value)
        {
            if (!entity.Get<MetadataComponent>().Attributes.TryGetValue(name, out object objValue))
            {
                value = default;
                return false;
            }

            if (objValue is not TValue)
            {
                value = default;
                return false;
            }

            value = (TValue)objValue;
            return true;
        }

        #endregion Private Methods
    }
}
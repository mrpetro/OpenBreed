using OpenBreed.Components.Physics;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenTK;

namespace OpenBreed.Systems.Physics.Helpers
{
    /// <summary>
    /// Package of entity and dynamic body related components
    /// </summary>
    internal class DynamicPack
    {
        #region Internal Constructors

        internal DynamicPack(int entityId,
            BodyComponent body,
            PositionComponent position,
            VelocityComponent velocity)
        {
            EntityId = entityId;
            Body = body;
            Position = position;
            Velocity = velocity;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int EntityId { get; }
        internal BodyComponent Body { get; }
        internal PositionComponent Position { get; }
        internal VelocityComponent Velocity { get; }

        internal Box2 Aabb
        {
            get
            {
                return Body.Aabb;
            }
        }

        #endregion Internal Properties
    }
}
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenTK;

namespace OpenBreed.Wecs.Systems.Physics.Helpers
{
    /// <summary>
    /// Package of entity and static body related components
    /// </summary>
    internal class StaticPack
    {
        #region Internal Constructors

        internal StaticPack(int entityId,
            BodyComponent body,
            PositionComponent position)
        {
            EntityId = entityId;
            Body = body;
            Position = position;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int EntityId { get; }
        internal BodyComponent Body { get; }
        internal PositionComponent Position { get; }

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
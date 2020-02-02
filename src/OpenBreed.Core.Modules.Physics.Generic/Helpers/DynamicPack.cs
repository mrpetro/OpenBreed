using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenTK;

namespace OpenBreed.Core.Modules.Physics.Helpers
{
    /// <summary>
    /// Package of entity and dynamic body related components
    /// </summary>
    internal class DynamicPack
    {
        #region Internal Constructors

        internal DynamicPack(int entityId,
            Body body,
            Position position,
            Velocity velocity,
            IShapeComponent shape)
        {
            EntityId = entityId;
            Body = body;
            Position = position;
            Velocity = velocity;
            Shape = shape;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int EntityId { get; }
        internal Body Body { get; }
        internal Position Position { get; }
        internal Velocity Velocity { get; }
        internal IShapeComponent Shape { get; }

        internal Box2 Aabb { get { return Shape.Aabb.Translated(Position.Value); } }

        #endregion Internal Properties
    }
}
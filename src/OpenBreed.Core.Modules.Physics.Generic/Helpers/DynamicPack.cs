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

        internal DynamicPack(IEntity entity,
            IDynamicBody body,
            IPosition position,
            IVelocity velocity,
            IShapeComponent shape)
        {
            Entity = entity;
            Body = body;
            Position = position;
            Velocity = velocity;
            Shape = shape;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IEntity Entity { get; }
        internal IDynamicBody Body { get; }
        internal IPosition Position { get; }
        internal IVelocity Velocity { get; }
        internal IShapeComponent Shape { get; }

        internal Box2 Aabb { get { return Shape.Aabb.Translated(Position.Value); } }

        #endregion Internal Properties
    }
}
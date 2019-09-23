using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenTK;

namespace OpenBreed.Core.Modules.Physics.Helpers
{
    /// <summary>
    /// Package of entity and static body related components
    /// </summary>
    internal class StaticPack
    {
        #region Internal Constructors

        internal StaticPack(IEntity entity,
            IBody body,
            Position position,
            IShapeComponent shape)
        {
            Entity = entity;
            Body = body;
            Position = position;
            Shape = shape;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IEntity Entity { get; }
        internal IBody Body { get; }
        internal Position Position { get; }
        internal IShapeComponent Shape { get; }

        internal Box2 Aabb { get { return Shape.Aabb.Translated(Position.Value); } }


        #endregion Internal Properties
    }
}
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
            GridPosition gridPosition,
            IShapeComponent shape)
        {
            Entity = entity;
            Body = body;
            GridPosition = gridPosition;
            Shape = shape;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IEntity Entity { get; }
        internal IBody Body { get; }
        internal GridPosition GridPosition { get; }
        internal IShapeComponent Shape { get; }

        #endregion Internal Properties
    }
}
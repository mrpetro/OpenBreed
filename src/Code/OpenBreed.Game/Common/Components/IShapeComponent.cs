using OpenTK;

namespace OpenBreed.Game.Common.Components
{
    /// <summary>
    /// Component interface of geometry shapes
    /// </summary>
    public interface IShapeComponent : IEntityComponent
    {
        #region Public Properties

        /// <summary>
        /// Axis-aligned bounding box of this shape
        /// </summary>
        Box2 Aabb { get; }

        #endregion Public Properties
    }
}
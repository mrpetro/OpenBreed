using OpenBreed.Game.Physics.Helpers;
using OpenTK;

namespace OpenBreed.Game.Common.Components
{
    public interface IShapeComponent : IEntityComponent
    {
        #region Public Properties

        Box2 Aabb { get; }

        #endregion Public Properties
    }
}
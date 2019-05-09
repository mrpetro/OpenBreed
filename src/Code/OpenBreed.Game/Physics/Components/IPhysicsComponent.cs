using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Physics.Helpers;
using OpenTK;

namespace OpenBreed.Game.Physics.Components
{
    /// <summary>
    /// Component interface which is dedicated for physics system
    /// </summary>
    public interface IPhysicsComponent : IEntityComponent
    {
        #region Public Properties

        Box2 Aabb { get; }

        #endregion Public Properties
    }
}
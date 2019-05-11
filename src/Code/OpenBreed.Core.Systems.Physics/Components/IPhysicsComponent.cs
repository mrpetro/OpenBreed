using OpenBreed.Core.Common.Components;
using OpenTK;

namespace OpenBreed.Core.Systems.Physics.Components
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
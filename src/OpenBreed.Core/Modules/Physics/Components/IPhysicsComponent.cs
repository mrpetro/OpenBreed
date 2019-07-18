using OpenBreed.Core.Common.Systems.Components;
using OpenTK;

namespace OpenBreed.Core.Modules.Physics.Components
{
    /// <summary>
    /// Component interface which is dedicated for physics system
    /// </summary>
    public interface IPhysicsComponent : IEntityComponent
    {
        #region Public Properties

        Box2 Aabb { get; set; }

        #endregion Public Properties
    }
}
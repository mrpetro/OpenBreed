using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenTK;

namespace OpenBreed.Core.Modules.Physics.Components
{
    /// <summary>
    /// Component interface which is dedicated for physics system
    /// </summary>
    public interface IPhysicsComponent : IEntityComponent
    {
        #region Public Properties

        Box2 Aabb { get; }

        #endregion Public Properties

        #region Public Methods

        void Deinitialize(IEntity entity);

        void Initialize(IEntity entity);

        #endregion Public Methods
    }
}
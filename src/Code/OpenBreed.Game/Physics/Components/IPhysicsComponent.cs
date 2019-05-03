using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Physics.Helpers;

namespace OpenBreed.Game.Physics.Components
{
    public interface IPhysicsComponent : IEntityComponent
    {
        #region Public Properties

        Aabb Aabb { get; }

        #endregion Public Properties
    }
}
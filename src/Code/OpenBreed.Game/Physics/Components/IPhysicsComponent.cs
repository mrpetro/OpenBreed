using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Physics.Helpers;
using OpenTK;

namespace OpenBreed.Game.Physics.Components
{
    public interface IPhysicsComponent : IEntityComponent
    {
        #region Public Properties

        Box2 Aabb { get; }

        /// <summary>
        /// Resolve collision with other component
        /// </summary>
        /// <param name="other">Other physics component</param>
        void Resolve(IPhysicsComponent other);

        #endregion Public Properties
    }
}
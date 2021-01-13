using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;

namespace OpenBreed.Physics.Interface
{
    public interface IPhysicsModule : ICoreModule
    {
        #region Public Properties

        IFixtureMan Fixturs { get; }

        IShapeMan Shapes { get; }

        ICollisionMan Collisions { get; }

        #endregion Public Properties
    }
}
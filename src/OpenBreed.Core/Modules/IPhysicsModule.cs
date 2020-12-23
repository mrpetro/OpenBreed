using OpenBreed.Core.Managers;

namespace OpenBreed.Core.Modules
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
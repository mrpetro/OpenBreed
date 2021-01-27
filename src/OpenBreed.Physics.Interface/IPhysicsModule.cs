using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Physics.Interface.Managers;

namespace OpenBreed.Physics.Interface
{
    public interface IPhysicsModule : ICoreModule
    {
        #region Public Properties

        IFixtureMan Fixturs { get; }

        IShapeMan Shapes { get; }

        ICollisionMan Collisions { get; }


        IPhysicalWorld CreateWorld();

        #endregion Public Properties
    }
}
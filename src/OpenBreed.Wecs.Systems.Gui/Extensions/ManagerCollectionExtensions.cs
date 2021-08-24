using OpenBreed.Common;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Wecs.Systems.Gui.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupPhysicsDebugSystem(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new PhysicsDebugDisplaySystem(manCollection.GetManager<IPrimitiveRenderer>()));
        }

        #endregion Public Methods
    }
}
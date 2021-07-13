using OpenBreed.Common;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Wecs.Systems.Gui.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupGuiSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new UiSystem(manCollection.GetManager<IInputsMan>()));
        }

        #endregion Public Methods
    }
}
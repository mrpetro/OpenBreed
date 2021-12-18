using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Wecs.Systems.Animation.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupAnimationSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();

            systemFactory.Register(() => new AnimatorSystem(manCollection.GetManager<IEntityMan>(),
                                                             manCollection.GetManager<IClipMan<Entity>>(),
                                                             manCollection.GetManager<ILogger>()));
        }

        #endregion Public Methods
    }
}
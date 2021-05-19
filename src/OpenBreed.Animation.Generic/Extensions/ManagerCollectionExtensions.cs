using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Logging;

namespace OpenBreed.Animation.Generic.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupAnimationManagers(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<IAnimationMan>(() => new AnimationMan(manCollection.GetManager<ILogger>()));
            manCollection.AddSingleton<IFrameUpdaterMan>(() => new FrameUpdaterMan(manCollection.GetManager<ILogger>()));
        }

        #endregion Public Methods
    }
}
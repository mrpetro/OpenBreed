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
            manCollection.AddSingleton<IAnimMan>(() => new AnimMan(manCollection.GetManager<ILogger>()));
        }

        #endregion Public Methods
    }
}
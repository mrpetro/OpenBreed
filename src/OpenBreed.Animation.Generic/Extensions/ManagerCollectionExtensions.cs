using OpenBreed.Animation.Generic.Data;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;

namespace OpenBreed.Animation.Generic.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupAnimationManagers(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<Interface.IClipMan>(() => new ClipMan(manCollection.GetManager<ILogger>()));
            manCollection.AddSingleton<IFrameUpdaterMan>(() => new FrameUpdaterMan(manCollection.GetManager<ILogger>()));
        }


        public static void SetupAnimationDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register(new AnimationDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
                                                                               managerCollection.GetManager<IClipMan>(),
                                                                               managerCollection.GetManager<IFrameUpdaterMan>()));
        }

        #endregion Public Methods
    }
}
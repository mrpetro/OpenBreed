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

        public static void SetupAnimationManagers<TObject>(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<Interface.IClipMan<TObject>>(() => new ClipMan<TObject>(manCollection.GetManager<ILogger>()));
            manCollection.AddSingleton<IFrameUpdaterMan<TObject>>(() => new FrameUpdaterMan<TObject>(manCollection.GetManager<ILogger>()));
        }


        public static void SetupAnimationDataLoader<TObject>(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register<IAnimationClipDataLoader<TObject>>(() => new AnimationClipDataLoader<TObject>(managerCollection.GetManager<IRepositoryProvider>(),
                                                                            managerCollection.GetManager<IClipMan<TObject>>(),
                                                                            managerCollection.GetManager<IFrameUpdaterMan<TObject>>(),
                                                                            managerCollection.GetManager<ILogger>()));
        }

        #endregion Public Methods
    }
}
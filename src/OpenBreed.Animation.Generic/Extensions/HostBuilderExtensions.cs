using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic.Builders;
using OpenBreed.Animation.Generic.Data;
using OpenBreed.Animation.Abstractions;
using OpenBreed.Animation.Abstractions.Builders;
using OpenBreed.Animation.Abstractions.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using System;

namespace OpenBreed.Animation.Generic.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupFrameUpdaterMan<TObject>(this IHostBuilder hostBuilder, Action<IFrameUpdaterMan<TObject>, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IFrameUpdaterMan<TObject>>((sp) =>
                {
                    var frameUpdaterMan = new FrameUpdaterMan<TObject>(sp.GetService<ILogger>());
                    action.Invoke(frameUpdaterMan, sp);
                    return frameUpdaterMan;
                });
            });
        }

        public static void SetupClipMan<TObject>(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IClipMan<TObject>, ClipMan<TObject>>();
            });
        }

        public static void SetupBuilders<TObject>(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IReadOnlyClipBuilder<TObject>, ReadOnlyClipBuilder<TObject>>();
            });
        }

        public static void SetupAnimationDataLoader<TObject>(this DataLoaderFactory dataLoaderFactory, IServiceProvider managerCollection)
        {
            dataLoaderFactory.Register<IReadOnlyClipDataLoader<TObject>>(() => new ReadOnlyClipDataLoader<TObject>(managerCollection.GetService<IRepositoryProvider>(),
                                                                            managerCollection.GetService<IClipMan<TObject>>(),
                                                                            managerCollection.GetService<IFrameUpdaterMan<TObject>>(),
                                                                            managerCollection.GetService<ILogger>()));
            dataLoaderFactory.Register<IEditableClipDataLoader<TObject>>(() => new EditableClipDataLoader<TObject>(managerCollection.GetService<IRepositoryProvider>(),
                                                                            managerCollection.GetService<IClipMan<TObject>>(),
                                                                            managerCollection.GetService<IFrameUpdaterMan<TObject>>(),
                                                                            managerCollection.GetService<ILogger>()));
        }

        #endregion Public Methods
    }
}
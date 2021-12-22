﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Animation.Generic.Data;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using System;

namespace OpenBreed.Animation.Generic.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupAnimationManagers<TObject>(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IClipMan<TObject>, ClipMan<TObject>>();
                services.AddSingleton<IFrameUpdaterMan<TObject>, FrameUpdaterMan<TObject>>();
            });
        }

        public static void SetupAnimationDataLoader<TObject>(this DataLoaderFactory dataLoaderFactory, IServiceProvider managerCollection)
        {
            dataLoaderFactory.Register<IAnimationClipDataLoader<TObject>>(() => new AnimationClipDataLoader<TObject>(managerCollection.GetService<IRepositoryProvider>(),
                                                                            managerCollection.GetService<IClipMan<TObject>>(),
                                                                            managerCollection.GetService<IFrameUpdaterMan<TObject>>(),
                                                                            managerCollection.GetService<ILogger>()));
        }

        #endregion Public Methods
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Audio.Interface.Data;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Audio.OpenAL.Data;
using OpenBreed.Audio.OpenAL.Managers;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Audio.OpenAL.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupOpenALManagers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ISoundMan, SoundMan>();
            });
        }

        //public static void SetupSoundSampleDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        //{
        //    dataLoaderFactory.Register<ISoundSampleDataLoader>(() => new SoundSampleDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
        //                                                     managerCollection.GetManager<AssetsDataProvider>(),
        //                                                     managerCollection.GetManager<IModelsProvider>(),
        //                                                     managerCollection.GetManager<ISoundMan>(),
        //                                                     managerCollection.GetManager<ILogger>()));
        //}
    }
}

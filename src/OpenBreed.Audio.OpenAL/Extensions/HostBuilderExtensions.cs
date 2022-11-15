using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using OpenBreed.Audio.Interface;
using OpenBreed.Audio.Interface.Data;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Audio.OpenAL.Data;
using OpenBreed.Audio.OpenAL.Managers;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
                services.AddSingleton<ISoundMan>((sp) =>
                {
                    var audioOptions = sp.GetService<IOptions<AudioSettings>>();

                    if (audioOptions.Value.DisableSound)
                        return NullSoundMan.Instance;
                    else
                        return new SoundMan(sp.GetService<ILogger>());
                });
            });
        }

        public static void SetupSoundSampleDataLoader(this DataLoaderFactory dataLoaderFactory, IServiceProvider managerCollection)
        {
            dataLoaderFactory.Register<ISoundSampleDataLoader>(() => new SoundSampleDataLoader(managerCollection.GetService<IRepositoryProvider>(),
                                                             managerCollection.GetService<AssetsDataProvider>(),
                                                             managerCollection.GetService<IModelsProvider>(),
                                                             managerCollection.GetService<ISoundMan>(),
                                                             managerCollection.GetService<ILogger>()));
        }
    }
}

using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Audio.OpenAL.Managers;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Audio.OpenAL.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void AddOpenALManagers(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<ISoundMan>(() => new SoundMan(manCollection.GetManager<ILogger>()));
        }
    }
}

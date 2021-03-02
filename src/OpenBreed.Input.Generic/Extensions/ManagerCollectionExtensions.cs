using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Input.Generic.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupGenericInputManagers(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<IInputsMan>(() => new InputsMan(manCollection.GetManager<IClientMan>()));

            manCollection.AddSingleton<IPlayersMan>(() => new PlayersMan(manCollection.GetManager<ILogger>(),
                                                                         manCollection.GetManager<IInputsMan>()));

        }
    }
}

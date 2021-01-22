using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core
{
    public abstract class CoreFactory
    {
        protected readonly DefaultManagerCollection manCollection = new DefaultManagerCollection();

        protected CoreFactory()
        {
            manCollection.AddSingleton<ILogger>(() => new DefaultLogger());

            manCollection.AddSingleton<ICommandsMan>(() => new CommandsMan(manCollection.GetManager<ICore>(),
                                                                              manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IEventsMan>(() => new EventsMan(manCollection.GetManager<ICore>()));
        }


    }
}

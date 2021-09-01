using OpenBreed.Common;
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

            manCollection.AddSingleton<ICommandsMan>(() => new CommandsMan(manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IEventsMan>(() => new EventsMan());

            manCollection.AddSingleton<JobsMan>(() => new JobsMan());

            manCollection.AddSingleton<IEventQueue>(() => new EventQueue(manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IMessagesMan>(() => new MessagesMan());
        }


    }
}

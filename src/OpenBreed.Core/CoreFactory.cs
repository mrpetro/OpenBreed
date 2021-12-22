using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        protected CoreFactory(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ILogger, DefaultLogger>();
                services.AddSingleton<IEventsMan, EventsMan>();
                services.AddSingleton<IJobsMan, JobsMan>();
                services.AddSingleton<IEventQueue, EventQueue>();
                services.AddSingleton<IMessagesMan, MessagesMan>();
            });

            //manCollection.AddSingleton<ILogger>(() => new DefaultLogger());

            //manCollection.AddSingleton<IEventsMan>(() => new EventsMan());

            //manCollection.AddSingleton<IJobsMan>(() => new JobsMan());

            //manCollection.AddSingleton<IEventQueue>(() => new EventQueue(manCollection.GetManager<ILogger>()));

            //manCollection.AddSingleton<IMessagesMan>(() => new MessagesMan());
        }


    }
}

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
        }
    }
}

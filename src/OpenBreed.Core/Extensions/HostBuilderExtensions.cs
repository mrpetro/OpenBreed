using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using System;

namespace OpenBreed.Core.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupCoreManagers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ILogger, DefaultLogger>();
                services.AddSingleton<IEventsMan, DefaultEventsMan>();

                services.AddSingleton<IJobsMan, JobsMan>();
                //services.AddSingleton<IEventQueue, EventQueue>();
                services.AddSingleton<IMessagesMan, MessagesMan>();
            });
        }

        public static void SetupTriggerMan(this IHostBuilder hostBuilder, Action<ITriggerMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ITriggerMan>((sp) =>
                {
                    var collisionMan = new DefaultTriggerMan(sp.GetService<ILogger>());
                    action.Invoke(collisionMan, sp);
                    return collisionMan;
                });
            });
        }

        #endregion Public Methods
    }
}
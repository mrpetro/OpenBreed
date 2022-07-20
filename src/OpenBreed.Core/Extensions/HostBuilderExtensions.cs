using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using System;

namespace OpenBreed.Core.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupDataGridFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IDataGridFactory, DefaultDataGridFactory>();
            });
        }

        public static void SetupCoreManagers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ILogger, DefaultLogger>();
                services.AddSingleton<IEventsMan, DefaultEventsMan>();
                services.AddSingleton<ITriggerMan, DefaultTriggerMan>();

                services.AddSingleton<IJobsMan, JobsMan>();
                //services.AddSingleton<IEventQueue, EventQueue>();
                services.AddSingleton<IMessagesMan, MessagesMan>();
            });
        }

        public static void SetupEventsManEx(this IHostBuilder hostBuilder, Action<IEventsManEx, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IEventsManEx>((sp) =>
                {
                    var collisionMan = new DefaultEventsManEx(sp.GetService<ILogger>());
                    action.Invoke(collisionMan, sp);
                    return collisionMan;
                });
            });
        }

        #endregion Public Methods
    }
}
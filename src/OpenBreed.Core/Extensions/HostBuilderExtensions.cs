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
                services.AddSingleton<IMessagesMan, MessagesMan>();
            });
        }

        #endregion Public Methods
    }
}
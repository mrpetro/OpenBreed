using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;

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
                services.AddSingleton<IEventsMan, EventsMan>();
                services.AddSingleton<IJobsMan, JobsMan>();
                services.AddSingleton<IEventQueue, EventQueue>();
                services.AddSingleton<IMessagesMan, MessagesMan>();
            });
        }

        #endregion Public Methods
    }
}
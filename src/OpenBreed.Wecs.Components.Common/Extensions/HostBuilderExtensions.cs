using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Common.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupCommonComponentFactories(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<PositionComponentFactory>();
                services.AddSingleton<GridPositionComponentFactory>();
                services.AddSingleton<VelocityComponentFactory>();
                services.AddSingleton<ThrustComponentFactory>();
                services.AddSingleton<TimerComponentFactory>();
                services.AddSingleton<FrameComponentFactory>();
                services.AddSingleton<MetadataComponentFactory>();
                services.AddSingleton<AngularPositionComponentFactory>();
                services.AddSingleton<AngularPositionTargetComponentFactory>();
                services.AddSingleton<AngularThrustComponentFactory>();
                services.AddSingleton<MessagingComponentFactory>();
                services.AddSingleton<FollowedComponentFactory>();
            });
        }
    }
}

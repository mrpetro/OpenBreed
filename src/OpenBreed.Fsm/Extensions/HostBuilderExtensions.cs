using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Fsm.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupFsmManager(this IHostBuilder hostBuilder, Action<IFsmMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IFsmMan>((sp) =>
                {
                    var fsmMan = new FsmMan();
                    action.Invoke(fsmMan, sp);
                    return fsmMan;
                });
            });
        }

        public static void SetupFsmComponentFactories(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<FsmComponentFactory,FsmComponentFactory>();
                services.AddTransient<FsmComponentBuilder, FsmComponentBuilder>();
            });
        }
    }
}

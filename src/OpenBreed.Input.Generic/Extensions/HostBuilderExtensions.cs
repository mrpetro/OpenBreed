using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Input.Generic.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupBinder(this IHostBuilder hostBuilder, Action<IActionBinder, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IActionBinder>((sp) =>
                {
                var keyBinder = default(IActionBinder);
                    action.Invoke(keyBinder, sp);
                    return keyBinder;
                });
            });
        }

        public static void SetupDefaultActionCodeProvider(this IHostBuilder hostBuilder, Action<DefaultActionCodeProvider, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IActionCodeProvider>((sp) =>
                {
                    var keyBinder = new DefaultActionCodeProvider();
                    action.Invoke(keyBinder, sp);
                    return keyBinder;
                });
            });
        }

        public static void SetupInputMan(this IHostBuilder hostBuilder, Action<IInputsMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IInputsMan>((sp) =>
                {
                    var inputsMan = new InputsMan(
                        sp.GetService<IViewClient>(),
                        sp.GetService<IEventsMan>());
                    action.Invoke(inputsMan, sp);
                    return inputsMan;
                });
            });
        }
    }
}

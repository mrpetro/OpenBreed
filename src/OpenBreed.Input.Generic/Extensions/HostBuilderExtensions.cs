using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Input.Generic.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupDefaultActionTriggerBinder(this IHostBuilder hostBuilder, Action<DefaultActionTriggerBinder, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IActionTriggerBinder>((sp) =>
                {
                var keyBinder = new DefaultActionTriggerBinder(sp.GetService<IInputsMan>());
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

        public static void SetupEditorInputMan(this IHostBuilder hostBuilder, Action<IInputsMan, IServiceProvider> action = null)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IInputsMan>((sp) =>
                {
                    var inputsMan = new EditorInputsMan();

                    if (action is not null)
                    {
                        action.Invoke(inputsMan, sp);
                    }

                    return inputsMan;
                });
            });
        }

        public static void SetupGameWindowInputMan(this IHostBuilder hostBuilder, Action<IInputsMan, IServiceProvider> action = null)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IInputsMan>((sp) =>
                {
                    var inputsMan = new InputsMan(
                        sp.GetService<GameWindow>(),
                        sp.GetService<IEventsMan>());

                    if (action is not null)
                    {
                        action.Invoke(inputsMan, sp);
                    }

                    return inputsMan;
                });
            });
        }
    }
}

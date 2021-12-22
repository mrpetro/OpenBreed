using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
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
        public static void SetupInputMan(this IHostBuilder hostBuilder, Action<IInputsMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IInputsMan>((sp) =>
                {
                    var inputsMan = new InputsMan(sp.GetService<IViewClient>());
                    action.Invoke(inputsMan, sp);
                    return inputsMan;
                });
            });
        }

        public static void SetupPlayersMan(this IHostBuilder hostBuilder, Action<IPlayersMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IPlayersMan>((sp) =>
                {
                    var playersMan = new PlayersMan(sp.GetService<ILogger>(),
                                                    sp.GetService<IInputsMan>());
                    action.Invoke(playersMan, sp);
                    return playersMan;
                });
            });
        }
    }
}

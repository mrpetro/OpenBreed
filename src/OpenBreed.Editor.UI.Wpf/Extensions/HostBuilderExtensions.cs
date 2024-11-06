using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Interface;
using OpenBreed.Editor.UI.Wpf.Options;
using OpenBreed.Editor.UI.Wpf.Palettes;
using OpenBreed.Editor.UI.Wpf.Tools;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Options;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace OpenBreed.Editor.UI.Wpf.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void ConfigureUIDispatcher(this IHostBuilder hostBuilder, Dispatcher dispatcher)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IDispatcher>((sp) => new UIDispatcher(dispatcher));
            });
        }

        public static void ConfigureOptionsForm(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddTransient((sp) =>
                {
                    var form = new OptionsForm();
                    var vm = sp.GetRequiredService<OptionsVM>();

                    vm.CloseAction = form.Close;
                    form.DataContext = vm;

                    return form;
                });
            });
        }

        public static void ConfigureAbtaPasswordGeneratorForm(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddTransient((sp) =>
                {
                    var form = new AbtaPasswordGeneratorForm();
                    var vm = sp.GetRequiredService<AbtaPasswordGeneratorVM>();

                    form.DataContext = vm;

                    return form;
                });
            });
        }

        #endregion Public Methods
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

namespace OpenBreed.Editor.UI.Wpf.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

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

        public static void ConfigureControlFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IControlFactory>((sp) =>
                {
                    var controlFactory = new ControlFactory();
                    controlFactory.RegisterWpfControls();
                    return controlFactory;
                });
            });
        }

        #endregion Public Methods
    }
}
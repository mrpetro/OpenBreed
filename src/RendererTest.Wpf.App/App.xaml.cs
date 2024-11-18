using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RendererTest.Wpf.App.VM;
using System.Configuration;
using System.Data;
using System.Windows;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Common.Extensions;
using OpenBreed.Core.Extensions;
using OpenBreed.Input.Generic.Extensions;
using OpenBreed.Common.Game.Extensions;
using OpenBreed.Common.Interface.Tools;
using OpenBreed.Gui.Interface.Extensions;
using OpenBreed.Editor.VM.Extensions;
using OpenBreed.Editor.UI.Wpf.Extensions;

namespace RendererTest.Wpf.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;

        private RendererVm CreateViewVm(IServiceProvider sp)
        {
            var serviceScope = sp.CreateScope();
            return serviceScope.ServiceProvider.GetRequiredService<RendererVm>();
        }

        public App()
        {
            ThreadTools.Initialize();

            var hostBuilder = new HostBuilder();

            hostBuilder.ConfigureUIDispatcher(this.Dispatcher);
            hostBuilder.SetupCommonViewModels();
            hostBuilder.SetupDefaultLogger();
            hostBuilder.ConfigureInteraction();

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                
                services.AddSingleton<MainVm>();
                services.AddScoped<RendererVm>();

                //Add business services as needed
                services.AddSingleton<Func<IServiceProvider, RendererVm>>((sp) =>
                {
                    return CreateViewVm(sp);
                });

                services.AddSingleton((sp) =>
                {
                    var mainWindow = new MainWindow();
                    var vm = sp.GetRequiredService<MainVm>();

                    mainWindow.DataContext = vm;

                    return mainWindow;
                });
            });

            hostBuilder.SetupCommonGameServices(isEditor: true);

            host = hostBuilder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var mainWindow = services.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occurred: " + ex.Message);
                }
            }
        }
    }

}

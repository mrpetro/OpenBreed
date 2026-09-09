using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Extensions;
using System;

namespace OpenBreed.Fsm.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupWecsFsmComponents(this IHostBuilder hostBuilder)
        {
            XmlComponentsList.RegisterAllAssemblyComponentTypes();
        }

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
                services.AddSingleton<IFsmMachineFactory<IEntity>, EntityFsmMachineFactory>();
            });
        }

        #endregion Public Methods
    }
}
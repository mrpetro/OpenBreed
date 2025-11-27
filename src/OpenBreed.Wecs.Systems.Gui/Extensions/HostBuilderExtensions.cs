using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using System;

namespace OpenBreed.Wecs.Systems.Gui.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupCollisionVisualizingOptions(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<CollisionVisualizingOptions>();
            });
        }

        public static void ConfigureGuiSystems(this IHostBuilder hostBuilder, bool isEditor)
        {
            //hostBuilder.ConfigureServices((hostContext, services) =>
            //{
            //    services.AddTransient<CollisionVisualizingSystem>();

            //    if (!isEditor)
            //    {
            //        services.AddTransient<CursorSystem>();
            //    }
            //});

            hostBuilder.SetupWecsAssemblySystems();
        }

        #endregion Public Methods
    }
}
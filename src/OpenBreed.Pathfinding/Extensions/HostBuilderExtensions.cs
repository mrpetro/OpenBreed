using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Pathfinding.Abstractions.Services;
using OpenBreed.Pathfinding.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Pathfinding.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupPathfindingService(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<PathfindingService>();
                services.AddSingleton<IPathfindingService>((sp) => sp.GetRequiredService<PathfindingService>());
                //services.AddHostedService<PathfindingService>((sp) => sp.GetRequiredService<PathfindingService>());
            });
        }

        #endregion Public Methods
    }
}
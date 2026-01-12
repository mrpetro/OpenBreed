using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupDynamicResolver(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DynamicResolver>();
            });
        }

        public static void SetupPhysicsSystemInitializer(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ISystemInitializer, PhysicsSystemInitializer>();
            });
        }

        public static void SetupPhysicsSystems(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupPhysicsSystemInitializer();
            hostBuilder.SetupWecsAssemblySystems();
        }

        #endregion Public Methods
    }
}
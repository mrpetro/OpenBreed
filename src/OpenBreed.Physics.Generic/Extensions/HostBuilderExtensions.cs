using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Physics.Generic.Managers;
using OpenBreed.Physics.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Generic.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupFixtureMan(this IHostBuilder hostBuilder, Action<IFixtureMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IFixtureMan>((sp) =>
                {
                    var fixtureMan = new FixtureMan(sp.GetService<ILogger>());
                    action.Invoke(fixtureMan, sp);
                    return fixtureMan;
                });
            });
        }

        public static void SetupShapeMan(this IHostBuilder hostBuilder, Action<IShapeMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IShapeMan>((sp) =>
                {
                    var shapeMan = new ShapeMan(sp.GetService<ILogger>());
                    action.Invoke(shapeMan, sp);
                    return shapeMan;
                });
            });
        }

        public static void SetupCollisionMan<TObject>(this IHostBuilder hostBuilder, Action<ICollisionMan<TObject>, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ICollisionMan<TObject>>((sp) =>
                {
                    var collisionMan = new CollisionMan<TObject>(sp.GetService<ILogger>());
                    action.Invoke(collisionMan, sp);
                    return collisionMan;
                });
            });
        }

        public static void SetupFixtureMan<TObject>(this IHostBuilder hostBuilder, Action<IFixtureMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IFixtureMan>((sp) =>
                {
                    var collisionMan = new FixtureMan(sp.GetService<ILogger>());
                    action.Invoke(collisionMan, sp);
                    return collisionMan;
                });
            });
        }

        public static void SetupBroadphaseFactory<TObject>(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IBroadphaseFactory, BroadphaseFactory>();
            });
        }
    }
}

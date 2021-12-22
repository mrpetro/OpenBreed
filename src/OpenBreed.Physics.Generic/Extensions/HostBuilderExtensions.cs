using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
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
        public static void SetupGenericPhysicsManagers<TObject>(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IShapeMan, ShapeMan>();
                services.AddSingleton<ICollisionMan<TObject>, CollisionMan<TObject>>();
                services.AddSingleton<IBroadphaseFactory, BroadphaseFactory>();
            });
        }
    }
}

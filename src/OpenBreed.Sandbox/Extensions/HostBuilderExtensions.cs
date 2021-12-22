using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Core;
using OpenBreed.Game;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Worlds;

namespace OpenBreed.Sandbox.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupViewClient(this IHostBuilder hostBuilder, int width, int height, string title)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IViewClient, OpenTKWindowClient>((s) => new OpenTKWindowClient(width, height, title));
            });
        }

        public static void SetupItemManager(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ItemsMan>();
            });
        }

        public static void SetupFixtureTypes(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<FixtureTypes>();
            });
        }

        public static void SetupViewportCreator(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ViewportCreator>();
            });
        }



        public static void SetupTeleportHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<TeleportHelper>();
            });
        }

        public static void SetupCameraHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<CameraHelper>();
            });
        }

        public static void SetupEnvironmentHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<EnvironmentHelper>();
            });
        }

        public static void SetupGenericCellHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<GenericCellHelper>();
            });
        }

        public static void SetupPickableHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<PickableHelper>();
            });
        }

        public static void SetupElectricGateHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ElectricGateHelper>();
            });
        }

        public static void SetupHudHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<HudHelper>();
            });
        }

        public static void SetupVanillaStatusBarHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<VanillaStatusBarHelper>();
            });
        }

        public static void SetupEntriesHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<EntriesHelper>();
            });
        }

        public static void SetupDoorHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DoorHelper>();
            });
        }

        public static void SetupScreenWorldHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ScreenWorldHelper>();
            });
        }

        public static void SetupGameHudWorldHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<GameHudWorldHelper>();
            });
        }

        public static void SetupDebugHudWorldHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DebugHudWorldHelper>();
            });
        }


        public static void SetupProjectileHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ProjectileHelper>();
            });
        }

        public static void SetupActorHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ActorHelper>();
            });
        }

        #endregion Public Methods
    }
}
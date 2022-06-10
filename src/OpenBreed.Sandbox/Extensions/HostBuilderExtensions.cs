using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Model.Maps;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;

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

        public static void SetupItemManager(this IHostBuilder hostBuilder, Action<ItemsMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ItemsMan>((sp) =>
                {
                    var itemsMan = new ItemsMan(sp.GetService<ILogger>());
                    action.Invoke(itemsMan, sp);
                    return itemsMan;
                });
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

        public static void SetupGameSmartcardWorldHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<GameSmartcardWorldHelper>();
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


        public static void SetupDataLoaderFactory(this IHostBuilder hostBuilder, Action<DataLoaderFactory, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IDataLoaderFactory>((sp) =>
                {
                    var dataLoaderFactory = new DataLoaderFactory();
                    action.Invoke(dataLoaderFactory, sp);
                    return dataLoaderFactory;
                });
            });
        }

        public static void SetupMapLegacyDataLoader(this DataLoaderFactory dataLoaderFactory, IServiceProvider managerCollection)
        {
            //NOTE: Needed for correct display of map in this coordinate system
            MapLayoutModel.FlippedY = true;

            dataLoaderFactory.Register<MapLegacyDataLoader>(() =>
            {
                var mapLegacyDataLoader = new MapLegacyDataLoader(dataLoaderFactory,
                                                              managerCollection.GetService<IRenderableFactory>(),
                                                              managerCollection.GetService<IEntityMan>(),
                                                              managerCollection.GetService<IRepositoryProvider>(),
                                                              managerCollection.GetService<MapsDataProvider>(),
                                                              managerCollection.GetService<ISystemFactory>(),
                                                              managerCollection.GetService<IWorldMan>(),
                                                              managerCollection.GetService<PalettesDataProvider>(),
                                                              managerCollection.GetService<IBroadphaseFactory>(),
                                                              managerCollection.GetService<ITileGridFactory>(),
                                                              managerCollection.GetService<ITileMan>(),
                                                              managerCollection.GetService<ILogger>(),
                                                              managerCollection.GetService<ITriggerMan>(),
                                                              managerCollection.GetService<IScriptMan>());

                mapLegacyDataLoader.RegisterEntityLoaders(managerCollection);
                return mapLegacyDataLoader;
            });

            dataLoaderFactory.Register<MapTxtDataLoader>(() =>
            {
                var mapTxtDataLoader = new MapTxtDataLoader(dataLoaderFactory,
                                                            managerCollection.GetService<IRenderableFactory>(),
                                                            managerCollection.GetService<IRepositoryProvider>(),
                                                            managerCollection.GetService<ISystemFactory>(),
                                                            managerCollection.GetService<IWorldMan>(),
                                                            managerCollection.GetService<PalettesDataProvider>(),
                                                            managerCollection.GetService<ActionSetsDataProvider>(),
                                                            managerCollection.GetService<IBroadphaseFactory>(),
                                                            managerCollection.GetService<ITileGridFactory>(),
                                                            managerCollection.GetService<ITileMan>(),
                                                            managerCollection.GetService<ILogger>());

                mapTxtDataLoader.RegisterEntityLoaders(managerCollection);
                return mapTxtDataLoader;
            });
        }


        public static void RegisterEntityLoaders(this IMapDataLoader mapLegacyDataLoader, IServiceProvider managerCollection)
        {
            mapLegacyDataLoader.Register("Unknown", new UnknownCellEntityLoader(managerCollection.GetService<GenericCellHelper>()));

            var levelExitCellLoader = new LevelExitCellLoader(managerCollection.GetService<ActorHelper>(),
                                                                managerCollection.GetService<EntriesHelper>());

            mapLegacyDataLoader.Register("MapExit1", levelExitCellLoader);
            mapLegacyDataLoader.Register("MapExit2", levelExitCellLoader);
            mapLegacyDataLoader.Register("MapExit3", levelExitCellLoader);

            var levelEntryCellLoader = new LevelEntryCellLoader(managerCollection.GetService<ActorHelper>(),
                                                                     managerCollection.GetService<EntriesHelper>());


            mapLegacyDataLoader.Register("MapEntry1", levelEntryCellLoader);
            mapLegacyDataLoader.Register("MapEntry2", levelEntryCellLoader);
            mapLegacyDataLoader.Register("MapEntry3", levelEntryCellLoader);

            //mapWorldDataLoader.Register(LevelEntryCellLoader.ENTRY_3, levelEntryCellLoader);
            //mapWorldDataLoader.Register(LevelEntryCellLoader.ENTRY_1, levelEntryCellLoader);
            //mapWorldDataLoader.Register(LevelEntryCellLoader.ENTRY_2, levelEntryCellLoader);

            var genericCellEntityLoader = new GenericCellEntityLoader(managerCollection.GetService<GenericCellHelper>());

            mapLegacyDataLoader.Register("Void", genericCellEntityLoader);
            mapLegacyDataLoader.Register("FullObstacle", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ActorOnlyObstacle", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleDownLeft", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleUpLeft", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleUpRight", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleDownRight", genericCellEntityLoader);

            var environmentCellLoader = new AnimatedCellLoader(managerCollection.GetService<EnvironmentHelper>());
            mapLegacyDataLoader.Register("TVFlickering", environmentCellLoader);
            mapLegacyDataLoader.Register("MonsterEating", environmentCellLoader);

            var doorCellEntityLoader = new DoorEntityLoader(managerCollection.GetService<DoorHelper>());
            mapLegacyDataLoader.Register("DoorStandard", doorCellEntityLoader);
            mapLegacyDataLoader.Register("DoorRed", doorCellEntityLoader);
            mapLegacyDataLoader.Register("DoorGreen", doorCellEntityLoader);
            mapLegacyDataLoader.Register("DoorBlue", doorCellEntityLoader);

            var electricGateEntityLoader = new ElectricGateEntityLoader(managerCollection.GetService<ElectricGateHelper>());
            mapLegacyDataLoader.Register("ElectricGateUp", electricGateEntityLoader);
            mapLegacyDataLoader.Register("ElectricGateDown", electricGateEntityLoader);
            mapLegacyDataLoader.Register("ElectricGateRight", electricGateEntityLoader);
            mapLegacyDataLoader.Register("ElectricGateLeft", electricGateEntityLoader);

            var genericItemEntityLoader = new GenericItemEntityLoader(managerCollection.GetService<PickableHelper>());

            mapLegacyDataLoader.Register("GenericItem", genericItemEntityLoader);

            var keycardCellEntityLoader = new KeycardEntityLoader(managerCollection.GetService<PickableHelper>());
            mapLegacyDataLoader.Register("KeycardRed", keycardCellEntityLoader);
            mapLegacyDataLoader.Register("KeycardGreen", keycardCellEntityLoader);
            mapLegacyDataLoader.Register("KeycardBlue", keycardCellEntityLoader);
            mapLegacyDataLoader.Register("KeycardSpecial", keycardCellEntityLoader);

            var smartCardCellEntityLoader = new SmartCardEntityLoader(managerCollection.GetService<PickableHelper>());
            mapLegacyDataLoader.Register("SmartCard1", smartCardCellEntityLoader);
            mapLegacyDataLoader.Register("SmartCard2", smartCardCellEntityLoader);
            mapLegacyDataLoader.Register("SmartCard3", smartCardCellEntityLoader);


            var teleportLoader = new TeleportCellEntityLoader(managerCollection.GetService<TeleportHelper>(),
                                                    managerCollection.GetService<ILogger>());

            mapLegacyDataLoader.Register("TeleportEntry", teleportLoader);
            mapLegacyDataLoader.Register("TeleportExit", teleportLoader);

            var landMineEntityLoader = new LandMineEntityLoader(managerCollection.GetService<GenericCellHelper>(),
                                        managerCollection.GetService<ILogger>());

            mapLegacyDataLoader.Register("LandMine", landMineEntityLoader);
        }




        #endregion Public Methods
    }
}
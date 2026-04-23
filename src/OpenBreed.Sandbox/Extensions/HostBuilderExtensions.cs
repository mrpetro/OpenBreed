using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Audio.Abstractions;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Input.Abstractions;
using OpenBreed.Model.Maps;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Services;
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Gui.Systems;
using OpenBreed.Wecs.Services;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.CommandLine;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace OpenBreed.Sandbox.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupLuaConsoleInput(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
             {
                 services.AddHostedService<LuaInputConsole>();
             });
        }

        public static void SetupGameWindow(this IHostBuilder hostBuilder, int width, int height, string title)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                var gameWindowSettings = new GameWindowSettings()
                {
                    UpdateFrequency = 60
                };

                var nativeWindowSettings = new NativeWindowSettings()
                {
                    ClientSize = new Vector2i(width, height),
                    RedBits = 8,
                    GreenBits = 8,
                    BlueBits = 8,
                    AlphaBits = 8,
                    DepthBits = 24,
                    StencilBits = 8,
                    Title = title,
                    Flags = ContextFlags.ForwardCompatible,
                    Vsync = VSyncMode.On
                };

                services.AddSingleton((sp) => new GameWindow(gameWindowSettings, nativeWindowSettings));
            });
        }

        public static void SetupWeaponsMan(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IWeaponMan, WeaponMan>();
            });
        }

        public static void SetupGameHudWorldHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<SetupHelper>();
            });
        }

        public static void SetupSandboxSystems(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupWecsAssemblySystems();
        }

        public static void SetupWecsSandboxComponents(this IHostBuilder hostBuilder)
        {
            XmlComponentsList.RegisterAllAssemblyComponentTypes();
            hostBuilder.SetupWecsAssemblyComponentFactories();
        }

        public static void SetupMapLegacyDataLoader(this IHostBuilder hostBuilder)
        {   
            //NOTE: Needed for correct display of map in this coordinate system
            MapLayoutModel.FlippedY = true;

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IMapDataLoader>((sp) => {
                    var mapLegacyDataLoader = new MapLegacyDataLoader(sp.GetService<IDataLoaderFactory>(),
                                                                  sp.GetService<IEntityMan>(),
                                                                  sp.GetService<IRepositoryProvider>(),
                                                                  sp.GetService<MapsDataProvider>(),
                                                                  sp.GetService<ISystemFactory>(),
                                                                  sp.GetService<IWorldMan>(),
                                                                  sp.GetService<PalettesDataProvider>(),
                                                                  sp.GetService<IBroadphaseFactory>(),
                                                                  sp.GetService<ITileGridFactory>(),
                                                                  sp.GetService<IDataGridFactory>(),
                                                                  sp.GetService<ITileMan>(),
                                                                  sp.GetService<IPaletteMan>(),
                                                                  sp.GetService<ILogger>(),
                                                                  sp.GetService<ITriggerMan>(),
                                                                  sp.GetService<IScriptMan>(),
                                                                  sp.GetService<IEntityFactory>(),
                                                                  sp.GetService<IBuilderFactory>());

                    mapLegacyDataLoader.RegisterEntityLoaders(sp);
                    return mapLegacyDataLoader;
                });
            });
        }

        public static void RegisterEntityLoaders(this IMapDataLoader mapLegacyDataLoader, IServiceProvider managerCollection)
        {
            mapLegacyDataLoader.Register("Unknown", new UnknownCellEntityLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>()));

            var levelExitCellLoader = new LevelExitCellLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());

            mapLegacyDataLoader.Register("MapExit1", levelExitCellLoader);
            mapLegacyDataLoader.Register("MapExit2", levelExitCellLoader);
            mapLegacyDataLoader.Register("MapExit3", levelExitCellLoader);

            var levelEntryCellLoader = new LevelEntryCellLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());

            mapLegacyDataLoader.Register("MapEntry1", levelEntryCellLoader);
            mapLegacyDataLoader.Register("MapEntry2", levelEntryCellLoader);
            mapLegacyDataLoader.Register("MapEntry3", levelEntryCellLoader);

            //mapWorldDataLoader.Register(LevelEntryCellLoader.ENTRY_3, levelEntryCellLoader);
            //mapWorldDataLoader.Register(LevelEntryCellLoader.ENTRY_1, levelEntryCellLoader);
            //mapWorldDataLoader.Register(LevelEntryCellLoader.ENTRY_2, levelEntryCellLoader);

            var genericCellEntityLoader = new GenericCellEntityLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());

            mapLegacyDataLoader.Register("Void", genericCellEntityLoader);
            mapLegacyDataLoader.Register("FullObstacle", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ActorOnlyObstacle", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleDownLeft", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleUpLeft", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleUpRight", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleDownRight", genericCellEntityLoader);

            var environmentCellLoader = new AnimatedCellLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());

            mapLegacyDataLoader.Register("TVFlickering", environmentCellLoader);
            mapLegacyDataLoader.Register("MonsterEating", environmentCellLoader);
            mapLegacyDataLoader.Register("L1/ShipSmoke", environmentCellLoader);

            var doorCellEntityLoader = new DoorEntityLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());

            mapLegacyDataLoader.Register("DoorStandard", doorCellEntityLoader);
            mapLegacyDataLoader.Register("DoorRed", doorCellEntityLoader);
            mapLegacyDataLoader.Register("DoorGreen", doorCellEntityLoader);
            mapLegacyDataLoader.Register("DoorBlue", doorCellEntityLoader);

            var electricGateEntityLoader = new ElectricGateEntityLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());
            mapLegacyDataLoader.Register("ElectricGateUp", electricGateEntityLoader);
            mapLegacyDataLoader.Register("ElectricGateDown", electricGateEntityLoader);
            mapLegacyDataLoader.Register("ElectricGateRight", electricGateEntityLoader);
            mapLegacyDataLoader.Register("ElectricGateLeft", electricGateEntityLoader);

            var genericItemEntityLoader = new GenericItemEntityLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());

            mapLegacyDataLoader.Register("GenericItem", genericItemEntityLoader);

            var keycardCellEntityLoader = new KeycardEntityLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());
            mapLegacyDataLoader.Register("Keycard1", keycardCellEntityLoader);
            mapLegacyDataLoader.Register("Keycard2", keycardCellEntityLoader);
            mapLegacyDataLoader.Register("Keycard3", keycardCellEntityLoader);
            mapLegacyDataLoader.Register("KeycardSpecial", keycardCellEntityLoader);

            var smartCardCellEntityLoader = new SmartCardEntityLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());
            mapLegacyDataLoader.Register("SmartCard1", smartCardCellEntityLoader);
            mapLegacyDataLoader.Register("SmartCard2", smartCardCellEntityLoader);
            mapLegacyDataLoader.Register("SmartCard3", smartCardCellEntityLoader);

            var teleportLoader = new TeleportCellEntityLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());

            mapLegacyDataLoader.Register("TeleportEntry", teleportLoader);
            mapLegacyDataLoader.Register("TeleportExit", teleportLoader);

            var landMineEntityLoader = new LandMineEntityLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>(),
                managerCollection.GetService<ILogger>());

            mapLegacyDataLoader.Register("LandMine", landMineEntityLoader);

            var heavyTurretEntityLoader = new TurretEntryLoader(managerCollection.GetRequiredService<IWorldMan>(),
                managerCollection.GetRequiredService<IEntityFactory>());

            mapLegacyDataLoader.Register("HeavyTurret", heavyTurretEntityLoader);
        }

        public static void SetupCommandLine(this IHostBuilder hostBuilder, string[] args)
        {
            hostBuilder.ConfigureServices((sc) =>
            {
                var dbFilePathOption = new Option<string>("--dbFilePath")
                {
                    Description = "Path to the game database file",
                    DefaultValueFactory = (a) => "db.xml"
                };

                var legacyFolderPathOption = new Option<string>("--legacyFolderPath")
                {
                    Description = "Path to legacy game resources folder."
                };

                var startingLevelOption = new Option<string>("--startingLevelName")
                {
                    Description = "Name of the starting level."
                };

                var disableAudioOption = new Option<bool>("--disableAudio")
                {
                    Description = "Disable all game audio."
                };

                var rootCommand = new RootCommand
                {
                    dbFilePathOption,
                    legacyFolderPathOption,
                    startingLevelOption,
                    disableAudioOption
                };

                ConfigureXmlDbSettings(rootCommand, args, (result) =>
                {
                    sc.Configure<XmlDbSettings>(xmlDbSettings =>
                    {
                        xmlDbSettings.DbFilePath = result.GetValue(dbFilePathOption);
                    });

                    sc.Configure<XmlEntityTemplateLoaderSettings>(xmlDbSettings =>
                    {
                        xmlDbSettings.DataDirPath = Path.GetDirectoryName(result.GetValue(dbFilePathOption));
                    });

                    sc.Configure<EnvironmentSettings>(settings =>
                    {
                        settings.LegacyFolderPath = result.GetValue(legacyFolderPathOption);
                    });

                    sc.Configure<GameSettings>(settings =>
                    {
                        settings.StartingLevelName = result.GetValue(startingLevelOption);
                    });

                    sc.Configure<AudioSettings>(settings =>
                    {
                        settings.DisableSound = result.GetValue(disableAudioOption);
                    });
                });
            });
        }

        #endregion Public Methods

        #region Private Methods

        private static void ConfigureXmlDbSettings(
            RootCommand rootCommand,
            string[] args,
            Action<ParseResult> resultProvider)
        {
            rootCommand.SetAction((parseResult) =>
            {
                resultProvider.Invoke(parseResult);
            });

            var parseResult = rootCommand.Parse(args);
            parseResult.Invoke();
        }

        #endregion Private Methods
    }
}
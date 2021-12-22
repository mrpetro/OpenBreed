using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Game;
using OpenBreed.Input.Interface;
using OpenBreed.Model.Maps;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Sandbox.Worlds.Wecs.Systems;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Audio;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Gui;
using OpenBreed.Wecs.Systems.Physics;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Worlds;
using OpenTK;

namespace OpenBreed.Sandbox.Extensions
{
    public static class SystemCollectionExtensions
    {
        #region Public Methods

        public static void SetupViewClient(this IManagerCollection manCollection, int width, int height, string title)
        {
            manCollection.AddSingleton<IViewClient>(() => new OpenTKWindowClient(width, height, title));
        }

        public static void SetupGameScriptingApi(this IManagerCollection manCollection)
        {
            var scriptMan = manCollection.GetManager<IScriptMan>();

            scriptMan.Expose("Worlds", manCollection.GetManager<IWorldMan>());
            scriptMan.Expose("Entities", manCollection.GetManager<IEntityMan>());
            scriptMan.Expose("Inputs", manCollection.GetManager<IInputsMan>());
            scriptMan.Expose("Logging", manCollection.GetManager<ILogger>());
            scriptMan.Expose("Players", manCollection.GetManager<IPlayersMan>());
        }

        public static void SetupUnknownMapCellDisplaySystem(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new UnknownMapCellDisplaySystem(manCollection.GetManager<IPrimitiveRenderer>(),
                                                                         manCollection.GetManager<IFontMan>()));
        }

        public static void SetupGroupMapCellDisplaySystem(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new GroupMapCellDisplaySystem(manCollection.GetManager<IPrimitiveRenderer>(),
                                                                         manCollection.GetManager<IFontMan>()));
        }

        public static void SetupItemManager(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<ItemsMan>(() => new ItemsMan(manCollection.GetManager<ILogger>()));
        }


        public static void RegisterEntityLoaders(this IMapDataLoader mapLegacyDataLoader, IManagerCollection managerCollection)
        {
            mapLegacyDataLoader.Register("Unknown", new UnknownCellEntityLoader(managerCollection.GetManager<GenericCellHelper>()));

            var levelExitCellLoader = new LevelExitCellLoader(managerCollection.GetManager<ActorHelper>(),
                                                                managerCollection.GetManager<EntriesHelper>());

            mapLegacyDataLoader.Register("MapExit1", levelExitCellLoader);
            mapLegacyDataLoader.Register("MapExit2", levelExitCellLoader);
            mapLegacyDataLoader.Register("MapExit3", levelExitCellLoader);

            var levelEntryCellLoader = new LevelEntryCellLoader(managerCollection.GetManager<ActorHelper>(),
                                                                     managerCollection.GetManager<EntriesHelper>());


            mapLegacyDataLoader.Register("MapEntry1", levelEntryCellLoader);
            mapLegacyDataLoader.Register("MapEntry2", levelEntryCellLoader);
            mapLegacyDataLoader.Register("MapEntry3", levelEntryCellLoader);

            //mapWorldDataLoader.Register(LevelEntryCellLoader.ENTRY_3, levelEntryCellLoader);
            //mapWorldDataLoader.Register(LevelEntryCellLoader.ENTRY_1, levelEntryCellLoader);
            //mapWorldDataLoader.Register(LevelEntryCellLoader.ENTRY_2, levelEntryCellLoader);

            var genericCellEntityLoader = new GenericCellEntityLoader(managerCollection.GetManager<GenericCellHelper>());

            mapLegacyDataLoader.Register("Void", genericCellEntityLoader);
            mapLegacyDataLoader.Register("FullObstacle", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ActorOnlyObstacle", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleDownLeft", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleUpLeft", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleUpRight", genericCellEntityLoader);
            mapLegacyDataLoader.Register("ObstacleDownRight", genericCellEntityLoader);

            var environmentCellLoader = new AnimatedCellLoader(managerCollection.GetManager<EnvironmentHelper>());
            mapLegacyDataLoader.Register("TVFlickering", environmentCellLoader);
            mapLegacyDataLoader.Register("MonsterEating", environmentCellLoader);

            var doorCellEntityLoader = new DoorEntityLoader(managerCollection.GetManager<DoorHelper>());
            mapLegacyDataLoader.Register("DoorStandard", doorCellEntityLoader);
            mapLegacyDataLoader.Register("DoorRed", doorCellEntityLoader);
            mapLegacyDataLoader.Register("DoorGreen", doorCellEntityLoader);
            mapLegacyDataLoader.Register("DoorBlue", doorCellEntityLoader);

            var electricGateEntityLoader = new ElectricGateEntityLoader(managerCollection.GetManager<ElectricGateHelper>());
            mapLegacyDataLoader.Register("ElectricGateUp", electricGateEntityLoader);
            mapLegacyDataLoader.Register("ElectricGateDown", electricGateEntityLoader);
            mapLegacyDataLoader.Register("ElectricGateRight", electricGateEntityLoader);
            mapLegacyDataLoader.Register("ElectricGateLeft", electricGateEntityLoader);

            var genericItemEntityLoader = new GenericItemEntityLoader(managerCollection.GetManager<PickableHelper>());

            mapLegacyDataLoader.Register("GenericItem", genericItemEntityLoader);

            var keycardCellEntityLoader = new KeycardEntityLoader(managerCollection.GetManager<PickableHelper>());
            mapLegacyDataLoader.Register("KeycardRed", keycardCellEntityLoader);
            mapLegacyDataLoader.Register("KeycardGreen", keycardCellEntityLoader);
            mapLegacyDataLoader.Register("KeycardBlue", keycardCellEntityLoader);
            mapLegacyDataLoader.Register("KeycardSpecial", keycardCellEntityLoader);

            var smartCardCellEntityLoader = new SmartCardEntityLoader(managerCollection.GetManager<PickableHelper>());
            mapLegacyDataLoader.Register("SmartCard1", smartCardCellEntityLoader);
            mapLegacyDataLoader.Register("SmartCard2", smartCardCellEntityLoader);
            mapLegacyDataLoader.Register("SmartCard3", smartCardCellEntityLoader);


            var teleportLoader = new TeleportCellEntityLoader(managerCollection.GetManager<TeleportHelper>(),
                                                    managerCollection.GetManager<ILogger>());

            mapLegacyDataLoader.Register("TeleportEntry", teleportLoader);
            mapLegacyDataLoader.Register("TeleportExit", teleportLoader);
        }

        public static void SetupMapLegacyDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            //NOTE: Needed for correct display of map in this coordinate system
            MapLayoutModel.FlippedY = true;

            dataLoaderFactory.Register<MapLegacyDataLoader>(() =>
            {
                var mapLegacyDataLoader = new MapLegacyDataLoader(dataLoaderFactory,
                                                              managerCollection.GetManager<IRenderableFactory>(),
                                                              managerCollection.GetManager<IEntityMan>(),
                                                              managerCollection.GetManager<IRepositoryProvider>(),
                                                              managerCollection.GetManager<MapsDataProvider>(),
                                                              managerCollection.GetManager<ISystemFactory>(),
                                                              managerCollection.GetManager<IWorldMan>(),
                                                              managerCollection.GetManager<PalettesDataProvider>(),
                                                              managerCollection.GetManager<IBroadphaseFactory>(),
                                                              managerCollection.GetManager<ITileGridFactory>(),
                                                              managerCollection.GetManager<ITileMan>(),
                                                              managerCollection.GetManager<ILogger>());

                mapLegacyDataLoader.RegisterEntityLoaders(managerCollection);
                return mapLegacyDataLoader;
            });

            dataLoaderFactory.Register<MapTxtDataLoader>(() =>
            {
                var mapTxtDataLoader = new MapTxtDataLoader(dataLoaderFactory,
                                                            managerCollection.GetManager<IRenderableFactory>(),
                                                            managerCollection.GetManager<IRepositoryProvider>(),
                                                            managerCollection.GetManager<ISystemFactory>(),
                                                            managerCollection.GetManager<IWorldMan>(),
                                                            managerCollection.GetManager<PalettesDataProvider>(),
                                                            managerCollection.GetManager<ActionSetsDataProvider>(),
                                                            managerCollection.GetManager<IBroadphaseFactory>(),
                                                            managerCollection.GetManager<ITileGridFactory>(),
                                                            managerCollection.GetManager<ITileMan>(),
                                                            managerCollection.GetManager<ILogger>());

                mapTxtDataLoader.RegisterEntityLoaders(managerCollection);
                return mapTxtDataLoader;
            });
        }

        //public static void SetupSpriteSetDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        //{
        //    dataLoaderFactory.Register<ISpriteAtlas>(() => new SpriteAtlasDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
        //                                                     managerCollection.GetManager<AssetsDataProvider>(),
        //                                                     managerCollection.GetManager<ITextureMan>(),
        //                                                     managerCollection.GetManager<ISpriteMan>()));
        //}

        public static void SetupGameWorldSystems(this WorldBuilder builder, ISystemFactory systemFactory)
        {
            builder.AddSystem(systemFactory.Create<WalkingControlSystem>());
            builder.AddSystem(systemFactory.Create<AiControlSystem>());
            builder.AddSystem(systemFactory.Create<WalkingControllerSystem>());
            builder.AddSystem(systemFactory.Create<AttackControllerSystem>());

            //Action
            builder.AddSystem(systemFactory.Create<MovementSystemVanilla>());
            builder.AddSystem(systemFactory.Create<DirectionSystemVanilla>());

            //builder.AddSystem(new FollowerSystem(core));
            builder.AddSystem(systemFactory.Create<DynamicBodiesAabbUpdaterSystem>());
            builder.AddSystem(systemFactory.Create<DynamicBodiesCollisionCheckSystem>());
            builder.AddSystem(systemFactory.Create<StaticBodiesSystem>());
            //builder.AddSystem(systemFactory.Create<CollisionResponseSystem>());

            builder.AddSystem(systemFactory.Create<FollowerSystem>());

            builder.AddSystem(systemFactory.Create<AnimatorSystem>());
            builder.AddSystem(systemFactory.Create<TimerSystem>());
            builder.AddSystem(systemFactory.Create<FsmSystem>());

            ////Audio
            builder.AddSystem(systemFactory.Create<SoundSystem>());

            //Video
            builder.AddSystem(systemFactory.Create<StampSystem>());
            builder.AddSystem(systemFactory.Create<TileSystem>());
            builder.AddSystem(systemFactory.Create<SpriteSystem>());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(systemFactory.Create<TextSystem>());

            builder.AddSystem(systemFactory.Create<PhysicsDebugDisplaySystem>());
            builder.AddSystem(systemFactory.Create<UnknownMapCellDisplaySystem>());
            //builder.AddSystem(systemFactory.Create<GroupMapCellDisplaySystem>());
            builder.AddSystem(systemFactory.Create<ViewportSystem>());


            //builder.AddModule(
        }

        public static void SetupDataLoaderFactory(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<IDataLoaderFactory>(() =>
            {
                var dataLoaderFactory = new DataLoaderFactory();

                dataLoaderFactory.SetupAnimationDataLoader<Entity>(managerCollection);
                dataLoaderFactory.SetupMapLegacyDataLoader(managerCollection);
                dataLoaderFactory.SetupTileSetDataLoader(managerCollection);
                dataLoaderFactory.SetupTileStampDataLoader(managerCollection);
                dataLoaderFactory.SetupSpriteSetDataLoader(managerCollection);
                dataLoaderFactory.SetupSoundSampleDataLoader(managerCollection);

                return dataLoaderFactory;
            });
        }

        public static void SetupSpriteComponentAnimator(this IManagerCollection managerCollection)
        {
            new SpriteComponentAnimator(managerCollection.GetManager<IFrameUpdaterMan<Entity>>(),
                                        managerCollection.GetManager<ISpriteMan>());
        }

        public static void SetupTeleportHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<TeleportHelper>(() => new TeleportHelper(manCollection.GetManager<IClipMan<Entity>>(),
                                                                        manCollection.GetManager<IWorldMan>(),
                                                                        manCollection.GetManager<IEntityMan>(),
                                                                        manCollection.GetManager<IEntityFactory>(),
                                                                        manCollection.GetManager<IEventsMan>(),
                                                                        manCollection.GetManager<ICollisionMan<Entity>>(),
                                                                        manCollection.GetManager<IBuilderFactory>(),
                                                                        manCollection.GetManager<IJobsMan>(),
                                                                        manCollection.GetManager<IShapeMan>()));
        }

        public static void SetupCameraHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<CameraHelper>(() => new CameraHelper(manCollection.GetManager<IClipMan<Entity>>(),
                                                                                manCollection.GetManager<IFrameUpdaterMan<Entity>>(),
                                                                                manCollection.GetManager<IDataLoaderFactory>(),
                                                                                manCollection.GetManager<IEntityFactory>()));
        }

        public static void SetupEnvironmentHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<EnvironmentHelper>(() => new EnvironmentHelper(manCollection.GetManager<IDataLoaderFactory>(),
                                                                                      manCollection.GetManager<IEntityFactory>(),
                                                                                      manCollection.GetManager<IBuilderFactory>()));
        }

        public static void SetupGenericCellHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<GenericCellHelper>(() => new GenericCellHelper(manCollection.GetManager<IDataLoaderFactory>(),
                                                                        manCollection.GetManager<IEntityFactory>()));
        }

        public static void SetupPickableHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<PickableHelper>(() => new PickableHelper(manCollection.GetManager<IDataLoaderFactory>(),
                                                                        manCollection.GetManager<IEntityFactory>()));
        }

        public static void SetupElectricGateHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<ElectricGateHelper>(() => new ElectricGateHelper(manCollection.GetManager<IDataLoaderFactory>(),
                                                                        manCollection.GetManager<IEntityFactory>()));
        }

        public static void SetupHudHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<HudHelper>(() => new HudHelper(manCollection.GetManager<IEntityFactory>(),
                                                                      manCollection.GetManager<IEntityMan>(),
                                                                      manCollection.GetManager<IViewClient>(),
                                                                      manCollection.GetManager<IJobsMan>(),
                                                                      manCollection.GetManager<IRenderingMan>()));


        }

        public static void SetupVanillaStatusBarHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<VanillaStatusBarHelper>(() => new VanillaStatusBarHelper(manCollection.GetManager<IEntityFactory>(),
                                                                      manCollection.GetManager<IEntityMan>(),
                                                                      manCollection.GetManager<IViewClient>(),
                                                                      manCollection.GetManager<IJobsMan>(),
                                                                      manCollection.GetManager<IRenderingMan>()));
        }

        public static void SetupEntriesHelper(this IManagerCollection manCollection)
        {

            manCollection.AddSingleton<EntriesHelper>(() => new EntriesHelper(manCollection.GetManager<IWorldMan>(),
                                                                                  manCollection.GetManager<IEntityMan>(),
                                                                                  manCollection.GetManager<IClipMan<Entity>>(),
                                                                                  manCollection.GetManager<IEntityFactory>(),
                                                                                  manCollection.GetManager<IEventsMan>(),
                                                                                  manCollection.GetManager<ICollisionMan<Entity>>(),
                                                                                  manCollection.GetManager<IJobsMan>(),
                                                                                  manCollection.GetManager<ViewportCreator>(),
                                                                                  manCollection.GetManager<IDataLoaderFactory>()));

        }

        public static void SetupDoorHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<DoorHelper>(() => new DoorHelper(manCollection.GetManager<IDataLoaderFactory>(),
                                                                        manCollection.GetManager<IEntityFactory>()));
        }





        public static void SetupScreenWorldHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<ScreenWorldHelper>(() => new ScreenWorldHelper(manCollection.GetManager<ISystemFactory>(),
                                                                                      manCollection.GetManager<IRenderableFactory>(),
                                                                                      manCollection.GetManager<IRenderingMan>(),
                                                                                      manCollection.GetManager<IWorldMan>(),
                                                                                      manCollection.GetManager<IEventsMan>(),
                                                                                      manCollection.GetManager<ViewportCreator>(),
                                                                                      manCollection.GetManager<IViewClient>()));


        }

        public static void SetupGameHudWorldHelper(this IManagerCollection manCollection)
        {

            manCollection.AddSingleton<GameHudWorldHelper>(() => new GameHudWorldHelper(manCollection.GetManager<ISystemFactory>(),
                                                                                        manCollection.GetManager<IRenderableFactory>(),
                                                                                        manCollection.GetManager<IWorldMan>(),
                                                                                        manCollection.GetManager<IFontMan>(),
                                                                                        manCollection.GetManager<IViewClient>(),
                                                                                        manCollection.GetManager<IEntityMan>(),
                                                                                        manCollection.GetManager<IEntityFactory>(),
                                                                                        manCollection.GetManager<VanillaStatusBarHelper>(),
                                                                                        manCollection.GetManager<CameraHelper>(),
                                                                                        manCollection.GetManager<IRepositoryProvider>(),
                                                                                        manCollection.GetManager<IDataLoaderFactory>(),
                                                                                        manCollection.GetManager<SpriteAtlasDataProvider>()));



        }

        public static void SetupDebugHudWorldHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<DebugHudWorldHelper>(() => new DebugHudWorldHelper(manCollection.GetManager<ISystemFactory>(),
                                                                                          manCollection.GetManager<IRenderableFactory>(),
                                                                                          manCollection.GetManager<IWorldMan>(),
                                                                                          manCollection.GetManager<IEntityMan>(),
                                                                                          manCollection.GetManager<HudHelper>(),
                                                                                          manCollection.GetManager<CameraHelper>(),
                                                                                          manCollection.GetManager<IViewClient>()));
        }


        public static void SetupProjectileHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<ProjectileHelper>(() => new ProjectileHelper(manCollection.GetManager<IClipMan<Entity>>(),
                                                                                    manCollection.GetManager<ICollisionMan<Entity>>(),
                                                                                    manCollection.GetManager<IEntityFactory>(),
                                                                                    manCollection.GetManager<DynamicResolver>()));
        }

        public static void SetupActorHelper(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<ActorHelper>(() => new ActorHelper(manCollection.GetManager<IClipMan<Entity>>(),
                                                                          manCollection.GetManager<ICollisionMan<Entity>>(),
                                                                          manCollection.GetManager<IPlayersMan>(),
                                                                          manCollection.GetManager<IDataLoaderFactory>(),
                                                                          manCollection.GetManager<IEntityFactory>(),
                                                                          manCollection.GetManager<DynamicResolver>(),
                                                                          manCollection.GetManager<FixtureTypes>()));
        }

        #endregion Public Methods
    }
}
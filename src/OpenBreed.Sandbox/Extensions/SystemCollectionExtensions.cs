using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Input.Interface;
using OpenBreed.Model.Maps;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.ElectricGate;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Sandbox.Loaders;
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
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Gui;
using OpenBreed.Wecs.Systems.Physics;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Worlds;
using OpenTK;

namespace OpenBreed.Sandbox.Extensions
{
    public static class SystemCollectionExtensions
    {
        #region Public Methods

        public static void SetupGameScriptingApi(this IManagerCollection manCollection)
        {
            var scriptMan = manCollection.GetManager<IScriptMan>();

            scriptMan.Expose("Worlds", manCollection.GetManager<IWorldMan>());
            scriptMan.Expose("Entities", manCollection.GetManager<IEntityMan>());
            scriptMan.Expose("Inputs", manCollection.GetManager<IInputsMan>());
            scriptMan.Expose("Logging", manCollection.GetManager<ILogger>());
            scriptMan.Expose("Players", manCollection.GetManager<IPlayersMan>());
        }

        public static void SetupSandboxBuilders(this IManagerCollection manCollection)
        {
            manCollection.AddTransient<CameraBuilder>(() => new CameraBuilder(manCollection.GetManager<IEntityMan>(),
                                                                              manCollection.GetManager<IBuilderFactory>()));

            //manCollection.AddTransient<WorldBlockBuilder>(() => new WorldBlockBuilder(manCollection.GetManager<ITileMan>(),
            //                                                                          manCollection.GetManager<IShapeMan>(),
            //                                                                          manCollection.GetManager<IEntityMan>(),
            //                                                                          manCollection.GetManager<IBuilderFactory>()));
        }

        public static void SetupPlayerCamera(this CameraBuilder cameraBuilder)
        {
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(320, 240);
        }

        public static void SetupMapEntityFactory(this EntityFactoryProvider entityFactoryProvider, IManagerCollection manCollection)
        {
            //entityFactoryProvider.Register<MapCellEntityFactory>(() => new MapCellEntityFactory());
            //mapEntityFactory.Register<IEntityFactory>(() => new WorldBlockBuilder(manCollection.GetManager<ITileMan>(),
            //                                                manCollection.GetManager<IFixtureMan>(),
            //                                                manCollection.GetManager<IEntityMan>(),
            //                                                manCollection.GetManager<BodyComponentBuilder>()));
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

        public static void SetupMapWorldDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            //NOTE: Needed for correct display of map in this coordinate system
            MapLayoutModel.FlippedY = true;

            dataLoaderFactory.Register<World>(() =>
            {
                var mapWorldDataLoader = new MapWorldDataLoader(dataLoaderFactory,
                                                              managerCollection.GetManager<IRepositoryProvider>(),
                                                              managerCollection.GetManager<MapsDataProvider>(),
                                                              managerCollection.GetManager<ISystemFactory>(),
                                                              managerCollection.GetManager<IWorldMan>(),
                                                              managerCollection.GetManager<PalettesDataProvider>(),
                                                              managerCollection.GetManager<IEntityFactoryProvider>(),
                                                              managerCollection.GetManager<IBroadphaseFactory>(),
                                                              managerCollection.GetManager<ITileGridFactory>(),
                                                              managerCollection.GetManager<ITileMan>());

                var genericCellEntityLoader = new GenericCellEntityLoader(managerCollection.GetManager<GenericCellHelper>());

                mapWorldDataLoader.Register(UnknownCellEntityLoader.UNKNOWN_CODE, new UnknownCellEntityLoader(managerCollection.GetManager<GenericCellHelper>()));

                mapWorldDataLoader.Register(GenericCellEntityLoader.VOID_CODE, genericCellEntityLoader);
                mapWorldDataLoader.Register(GenericCellEntityLoader.OBSTACLE_CODE, genericCellEntityLoader);

                var environmentCellLoader = new AnimatedCellLoader(managerCollection.GetManager<EnvironmentHelper>());
                mapWorldDataLoader.Register(AnimatedCellLoader.TV_FLICKERING_CODE, environmentCellLoader);
                mapWorldDataLoader.Register(AnimatedCellLoader.MONSTER_EATING_CODE, environmentCellLoader);

                mapWorldDataLoader.Register(LevelEntryCellLoader.CODE, new LevelEntryCellLoader(managerCollection.GetManager<ActorHelper>(),
                                                                         managerCollection.GetManager<WorldGateHelper>()));
                mapWorldDataLoader.Register(DoorCellEntityLoader.DOOR_STANDARD, new DoorCellEntityLoader(managerCollection.GetManager<DoorHelper>()));

                var electricGateEntityLoader = new ElectricGateCellEntityLoader(managerCollection.GetManager<ElectricGateHelper>());
                mapWorldDataLoader.Register(ElectricGateCellEntityLoader.PASS_UP, electricGateEntityLoader);
                mapWorldDataLoader.Register(ElectricGateCellEntityLoader.PASS_DOWN, electricGateEntityLoader);
                mapWorldDataLoader.Register(ElectricGateCellEntityLoader.PASS_RIGHT, electricGateEntityLoader);
                mapWorldDataLoader.Register(ElectricGateCellEntityLoader.PASS_LEFT, electricGateEntityLoader);

                var pickableCellEntityLoader = new ItemCellEntityLoader(managerCollection.GetManager<PickableHelper>());

                mapWorldDataLoader.Register(ItemCellEntityLoader.GENERIC_ITEM, pickableCellEntityLoader);
                mapWorldDataLoader.Register(ItemCellEntityLoader.KEYCARD_RED, pickableCellEntityLoader);
                mapWorldDataLoader.Register(ItemCellEntityLoader.KEYCARD_GREEN, pickableCellEntityLoader);
                mapWorldDataLoader.Register(ItemCellEntityLoader.KEYCARD_BLUE, pickableCellEntityLoader);
                mapWorldDataLoader.Register(ItemCellEntityLoader.KEYCARD_SPECIAL, pickableCellEntityLoader);
                mapWorldDataLoader.Register(ItemCellEntityLoader.SMARTCARD_1, pickableCellEntityLoader);
                mapWorldDataLoader.Register(ItemCellEntityLoader.SMARTCARD_2, pickableCellEntityLoader);
                mapWorldDataLoader.Register(ItemCellEntityLoader.SMARTCARD_3, pickableCellEntityLoader);

                var teleportLoader = new TeleportCellEntityLoader(managerCollection.GetManager<TeleportHelper>(),
                                                        managerCollection.GetManager<ILogger>());

                mapWorldDataLoader.Register(TeleportCellEntityLoader.ENTRY_CODE, teleportLoader);

                return mapWorldDataLoader;

            });


        }

        public static void SetupTileSetDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register<ITileAtlas>(() => new TileAtlasDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
                                                             managerCollection.GetManager<AssetsDataProvider>(),
                                                             managerCollection.GetManager<ITextureMan>(),
                                                             managerCollection.GetManager<ITileMan>()));
        }

        public static void SetupTileStampDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register<ITileStamp>(() => new TileStampDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
                                                             managerCollection.GetManager<AssetsDataProvider>(),
                                                             managerCollection.GetManager<ITextureMan>(),
                                                             managerCollection.GetManager<IStampMan>(),
                                                             managerCollection.GetManager<ITileMan>()));
        }

        public static void SetupSpriteSetDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register<ISpriteAtlas>(() => new SpriteAtlasDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
                                                             managerCollection.GetManager<AssetsDataProvider>(),
                                                             managerCollection.GetManager<ITextureMan>(),
                                                             managerCollection.GetManager<ISpriteMan>()));
        }

        public static void SetupGameWorldSystems(this WorldBuilder builder, ISystemFactory systemFactory)
        {
            builder.AddSystem(systemFactory.Create<WalkingControlSystem>());
            builder.AddSystem(systemFactory.Create<AiControlSystem>());
            builder.AddSystem(systemFactory.Create<WalkingControllerSystem>());
            builder.AddSystem(systemFactory.Create<AttackControllerSystem>());

            //Action
            builder.AddSystem(systemFactory.Create<MovementSystem>());
            builder.AddSystem(systemFactory.Create<DirectionSystem>());
            builder.AddSystem(systemFactory.Create<FollowerSystem>());
            //builder.AddSystem(new FollowerSystem(core));
            builder.AddSystem(systemFactory.Create<DynamicBodiesAabbUpdaterSystem>());
            builder.AddSystem(systemFactory.Create<DynamicBodiesCollisionCheckSystem>());
            builder.AddSystem(systemFactory.Create<StaticBodiesSystem>());
            //builder.AddSystem(systemFactory.Create<CollisionResponseSystem>());

            builder.AddSystem(systemFactory.Create<AnimatorSystem>());
            builder.AddSystem(systemFactory.Create<TimerSystem>());
            builder.AddSystem(systemFactory.Create<FsmSystem>());

            ////Audio
            //builder.AddSystem(systemFactory.Create<SoundSystem>());

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

        public static void SetupMapEntityFactory(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<IEntityFactoryProvider>(() =>
            {
                var mapEntityFactory = new EntityFactoryProvider();
                mapEntityFactory.SetupMapEntityFactory(managerCollection);
                return mapEntityFactory;
            });
        }

        public static void SetupDataLoaderFactory(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<IDataLoaderFactory>(() =>
            {
                var dataLoaderFactory = new DataLoaderFactory();

                dataLoaderFactory.SetupAnimationDataLoader(managerCollection);
                dataLoaderFactory.SetupMapWorldDataLoader(managerCollection);
                dataLoaderFactory.SetupTileSetDataLoader(managerCollection);
                dataLoaderFactory.SetupTileStampDataLoader(managerCollection);
                dataLoaderFactory.SetupSpriteSetDataLoader(managerCollection);

                return dataLoaderFactory;
            });
        }

        public static void SetupSpriteComponentAnimator(this IManagerCollection managerCollection)
        {
            new SpriteComponentAnimator(managerCollection.GetManager<IFrameUpdaterMan>(),
                                        managerCollection.GetManager<ISpriteMan>());
        }

        #endregion Public Methods
    }
}
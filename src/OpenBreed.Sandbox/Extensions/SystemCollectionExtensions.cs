﻿using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Worlds;
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
            scriptMan.Expose("Commands", manCollection.GetManager<ICommandsMan>());
            scriptMan.Expose("Inputs", manCollection.GetManager<IInputsMan>());
            scriptMan.Expose("Logging", manCollection.GetManager<ILogger>());
            scriptMan.Expose("Players", manCollection.GetManager<IPlayersMan>());
        }

        public static void SetupSandboxBuilders(this IManagerCollection manCollection)
        {
            manCollection.AddTransient<CameraBuilder>(() => new CameraBuilder(manCollection.GetManager<IEntityMan>(),
                                                                              manCollection.GetManager<CameraComponentBuilder>()));

            manCollection.AddTransient<WorldBlockBuilder>(() => new WorldBlockBuilder(manCollection.GetManager<ITileMan>(),
                                                                                      manCollection.GetManager<IFixtureMan>(),
                                                                                      manCollection.GetManager<IEntityMan>(),
                                                                                      manCollection.GetManager<BodyComponentBuilder>()));
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

        public static void SetupMapWorldDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register(new MapWorldDataLoader(dataLoaderFactory,
                                                              managerCollection.GetManager<IRepositoryProvider>(),
                                                              managerCollection.GetManager<MapsDataProvider>(),
                                                              managerCollection.GetManager<ISystemFactory>(),
                                                              managerCollection.GetManager<IWorldMan>(),
                                                              managerCollection.GetManager<WorldBlockBuilder>(),
                                                              managerCollection.GetManager<ICommandsMan>(),
                                                              managerCollection.GetManager<PalettesDataProvider>(),
                                                              managerCollection.GetManager<IEntityFactoryProvider>(),
                                                              managerCollection.GetManager<DoorHelper>()));
        }

        public static void SetupTileSetDataLoader(this DataLoaderFactory dataLoaderFactory, IManagerCollection managerCollection)
        {
            dataLoaderFactory.Register(new TileSetDataLoader(managerCollection.GetManager<IRepositoryProvider>(),
                                                             managerCollection.GetManager<AssetsDataProvider>(),
                                                             managerCollection.GetManager<ITextureMan>(),
                                                             managerCollection.GetManager<ITileMan>()));
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
            builder.AddSystem(systemFactory.Create<PhysicsSystem>());
            builder.AddSystem(systemFactory.Create<AnimationSystem>());
            builder.AddSystem(systemFactory.Create<TimerSystem>());
            builder.AddSystem(systemFactory.Create<FsmSystem>());

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            builder.AddSystem(systemFactory.Create<TileSystem>());
            builder.AddSystem(systemFactory.Create<SpriteSystem>());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(systemFactory.Create<TextSystem>());

            builder.AddSystem(systemFactory.Create<UiSystem>());

            builder.AddSystem(systemFactory.Create<ViewportSystem>());
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

                return dataLoaderFactory;
            });
        }

        #endregion Public Methods
    }
}
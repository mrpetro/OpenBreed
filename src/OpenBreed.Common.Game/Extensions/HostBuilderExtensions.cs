using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Input.Generic.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Input.Interface.Events;
using OpenBreed.Model.Maps;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Animation.Systems.Events;
using OpenBreed.Wecs.Core.Systems.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenBreed.Common.Extensions;
using System;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Physics.Generic.Extensions;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Core.Extensions;
using OpenBreed.Model.Extensions;
using OpenBreed.Rendering.Common.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Helpers;
using OpenBreed.Common.Game.Services;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Events;

namespace OpenBreed.Common.Game.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupItemManager(this IHostBuilder hostBuilder, Action<ItemsMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton((sp) =>
                {
                    var itemsMan = new ItemsMan(sp.GetService<ILogger>());
                    action.Invoke(itemsMan, sp);
                    return itemsMan;
                });
            });
        }

        public static void SetupGameServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IGameServices, GameServices>();
            });
        }

        public static void SetupCommonGameServices(this IHostBuilder hostBuilder, bool isEditor)
        {

            hostBuilder.SetupModelTools();

            hostBuilder.SetupCoreManagers();
            hostBuilder.SetupOpenALManagers();
            hostBuilder.SetupOpenGLManagers();
            hostBuilder.SetupCommonRenderingServices();
            hostBuilder.SetupGLRenderContextComponents();

            hostBuilder.ConfigureGraphicsDataLoaders();

            hostBuilder.SetupDataProviders(isEditor);
            hostBuilder.AddCommonServices();
            hostBuilder.SetupModelProvider();

            hostBuilder.SetupDefaultTypeAttributesProvider();

            hostBuilder.SetupDefaultActionCodeProvider((codeProvider, sp) =>
            {
                codeProvider.Register(PlayerActions.Fire);
            });

            if (isEditor)
            {
                hostBuilder.SetupEditorInputMan();
            }
            else
            {
                hostBuilder.SetupGameWindowInputMan();
            }

            hostBuilder.SetupDefaultActionTriggerBinder((keyBinder, sp) =>
            {
                keyBinder.Bind(PlayerActions.MoveLeft, Keys.Left);
                keyBinder.Bind(PlayerActions.MoveRight, Keys.Right);
                keyBinder.Bind(PlayerActions.MoveDown, Keys.Down);
                keyBinder.Bind(PlayerActions.MoveUp, Keys.Up);
                keyBinder.Bind(PlayerActions.Fire, Keys.RightControl);
            });




            hostBuilder.SetupLuaScripting((scriptMan, sp) =>
            {
                var eventsMan = sp.GetService<IEventsMan>();

                eventsMan.Subscribe<WorldInitializedEventArgs>(
                    (a) => scriptMan.TryInvokeFunction("WorldLoaded", a.WorldId));


                scriptMan.RegisterDelegateType(typeof(Action<IEntity, WorldPausedEventArgs>), typeof(LuaEntityEventHandler<WorldPausedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<IEntity, WorldUnpausedEventArgs>), typeof(LuaEntityEventHandler<WorldUnpausedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<IEntity, AnimFinishedEvent>), typeof(LuaEntityEventHandler<AnimFinishedEvent>));
                //scriptMan.RegisterDelegateType(typeof(Action<IEntity, ClientResizedEventArgs>), typeof(LuaEntityEventHandler<ClientResizedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<KeyDownEvent>), typeof(LuaEventHandler<KeyDownEvent>));
                scriptMan.RegisterDelegateType(typeof(Action<KeyUpEvent>), typeof(LuaEventHandler<KeyUpEvent>));

                scriptMan.Expose("Entities", sp.GetService<IEntityMan>());
                scriptMan.Expose("Sounds", sp.GetService<ISoundMan>());
                scriptMan.Expose("Triggers", sp.GetService<ITriggerMan>());
                scriptMan.Expose("Logging", sp.GetService<ILogger>());
                scriptMan.Expose("Rendering", sp.GetService<IRenderingMan>());
                scriptMan.Expose("Stamps", sp.GetService<IStampMan>());
                scriptMan.Expose("Clips", sp.GetService<IClipMan<IEntity>>());
                scriptMan.Expose("Shapes", sp.GetService<IShapeMan>());
                scriptMan.Expose("Items", sp.GetService<ItemsMan>());
                scriptMan.Expose("Texts", sp.GetService<TextsDataProvider>());
                scriptMan.Expose("Inputs", sp.GetService<IInputsMan>());
                scriptMan.Expose("Worlds", sp.GetService<IWorldMan>());
                scriptMan.Expose("Services", sp.GetService<IGameServices>());

                scriptMan.Expose("Coords", sp.GetService<CoordsTransformer>());

                var res = scriptMan.RunString(@"import('System')");
                res = scriptMan.RunString(@"import('OpenTK.Mathematics')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs', 'OpenBreed.Wecs.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Core.Components', 'OpenBreed.Wecs.Core.Components.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Core.Systems', 'OpenBreed.Wecs.Core.Systems.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Control.Systems', 'OpenBreed.Wecs.Control.Systems.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Control.Systems', 'OpenBreed.Wecs.Control.Systems.Helpers')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Control.Systems', 'OpenBreed.Wecs.Control.Systems')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Audio.Systems', 'OpenBreed.Wecs.Audio.Systems.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Rendering.Systems', 'OpenBreed.Wecs.Rendering.Systems.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Animation.Systems', 'OpenBreed.Wecs.Animation.Systems.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Physics.Systems', 'OpenBreed.Wecs.Physics.Systems.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Scripting.Systems', 'OpenBreed.Wecs.Scripting.Systems.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Gui.Systems', 'OpenBreed.Wecs.Gui.Systems.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Common', 'OpenBreed.Common.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Common.Game.Wecs', 'OpenBreed.Common.Game.Wecs.Extensions')");



                res = scriptMan.RunString(@"import('OpenBreed.Animation.Interface', 'OpenBreed.Animation.Interface.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Sandbox', 'OpenBreed.Sandbox.Extensions')");

                res = scriptMan.RunString(@"import('OpenBreed.Common.Game', 'OpenBreed.Common.Game')");

                res = scriptMan.RunString(@"import('OpenBreed.Sandbox', 'OpenBreed.Sandbox')");

                res = scriptMan.RunString(@"EntityTypes = {}");

                //var result = scriptMan.RunFile(@"D:\Projects\Programing\GIT\OpenBreed\OpenBreed.Common\src\OpenBreed.Database.Xml\Vanilla\Common\Scripts\Hud\FpsCounter.lua");


                //res = scriptMan.RunString(@"EntityTypes.FpsCounter.UpdateValue()");

            });

            hostBuilder.SetupFsmManager((fsmMan, sp) =>
            {
                //fsmMan.SetupButtonStates(sp);
                //fsmMan.SetupProjectileStates(sp);
                //fsmMan.SetupDoorStates(sp);
                //fsmMan.SetupPickableStates(sp);
                //fsmMan.SetupActorAttackingStates(sp);
                //fsmMan.SetupActorMovementStates(sp);
                //fsmMan.CreateTurretRotationStates(sp);
            });


            hostBuilder.SetupCollisionChecker();

            hostBuilder.SetupCollisionMan<IEntity>((collisionMan, sp) =>
            {
                collisionMan.RegisterAbtaColliders();
            });

            hostBuilder.SetupBroadphaseFactory<IEntity>();
            hostBuilder.SetupFixtureMan((s, a) => { });
            hostBuilder.SetupGameServices();
        }

        #endregion Public Methods
    }
}
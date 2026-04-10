using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Windows.Media.Media3D;

namespace OpenBreed.Sandbox.App.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupSandboxWecsSystems(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupRenderingSystems();
            hostBuilder.SetupScriptingSystems();
            hostBuilder.SetupAudioSystems();
            hostBuilder.SetupPhysicsSystems();
            hostBuilder.SetupCoreSystems();
            hostBuilder.SetupControlSystems();
            hostBuilder.ConfigureGuiSystems(isEditor: false);

            hostBuilder.SetupWecsCommonComponents();
            hostBuilder.SetupWecsPhysicsComponents();
            hostBuilder.SetupWecsRenderingComponents();
            hostBuilder.SetupWecsControlComponents();
            hostBuilder.SetupWecsAudioComponents();
            hostBuilder.SetupWecsFsmComponents();
            hostBuilder.SetupWecsScriptingComponents();
            hostBuilder.SetupWecsGuiComponents();
            hostBuilder.SetupWecsBase();

            hostBuilder.SetupBuilderFactory((builderFactory, sp) =>
            {
                builderFactory.SetupWecsPhysicsBuilders(sp);
                builderFactory.SetupWecsRenderingBuilders(sp);
                builderFactory.SetupWecsControlBuilders(sp);
            });
        }

        public static void SetupGameWindow(this IHostBuilder hostBuilder, int width, int height, string title)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                var gameWindowSettings = new GameWindowSettings()
                {
                    UpdateFrequency = 30
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

        #endregion Public Methods
    }
}
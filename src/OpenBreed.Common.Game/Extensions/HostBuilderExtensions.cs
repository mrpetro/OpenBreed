using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Model.Maps;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;

using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace OpenBreed.Sandbox.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods



 

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

        #endregion Public Methods
    }
}
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Scripting.Interface;
using System;

namespace OpenBreed.Wecs.Systems.Scripting.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupScriptingSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.RegisterSystem<ScriptRunningSystem>(
                () => new ScriptRunningSystem(
                    serviceProvider.GetService<IScriptMan>(),
                    serviceProvider.GetService<ILogger>()));
        }

        #endregion Public Methods
    }
}
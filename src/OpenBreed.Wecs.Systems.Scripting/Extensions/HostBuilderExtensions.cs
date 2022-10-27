using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Scripting.Interface;
using System;

namespace OpenBreed.Wecs.Systems.Scripting.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupScriptingSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.Register<ScriptRunningSystem>(() => new ScriptRunningSystem(serviceProvider.GetService<IScriptMan>()));
        }

        #endregion Public Methods
    }
}
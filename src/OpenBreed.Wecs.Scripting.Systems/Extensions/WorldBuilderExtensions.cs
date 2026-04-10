using OpenBreed.Wecs.Abstractions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Scripting.Systems.Extensions
{
    public static class WorldBuilderExtensions
    {
        #region Public Methods

        public static IWorldBuilder AddScriptingSystems(this IWorldBuilder builder)
        {
            builder.AddSystem<ScriptRunningSystem>();
            return builder;
        }

        #endregion Public Methods
    }
}
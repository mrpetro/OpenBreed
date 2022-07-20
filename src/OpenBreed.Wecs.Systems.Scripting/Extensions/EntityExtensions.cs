using OpenBreed.Wecs.Components.Scripting;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Scripting.Extensions
{
    public static class EntityExtensions
    {
        public static string GetScriptId(this Entity entity, string triggerName)
        {
            var sc = entity.Get<ScriptRunnerComponent>();

            var script = sc.Runs.FirstOrDefault(item => item.TriggerName == triggerName);

            if (script is null)
                return null;

            return script.ScriptId;
        }
    }
}

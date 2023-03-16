using OpenBreed.Common.Interface.Logging;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Scripting;
using OpenBreed.Wecs.Entities;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Scripting.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static string GetFunctionId(this IEntity entity, string triggerName)
        {
            var sc = entity.TryGet<ScriptComponent>();

            if (sc is null)
                return null;

            var hook = sc.SystemHooks.FirstOrDefault(item => item.TriggerName == triggerName);

            if (hook is null)
                return null;

            return hook.FunctionId;
        }

        public static void TryInvoke(this IEntity entity,
            IScriptMan scriptMan,
            ILogger logger,
            string triggerName, object arg = null)
        {
            var component = entity.TryGet<ScriptComponent>();

            if (component is null)
                return;

            var hook = component.SystemHooks.FirstOrDefault(item => item.TriggerName == triggerName);

            if (hook is null)
                return;

            var initFunction = scriptMan.GetFunction(hook.FunctionId);

            if (initFunction is null)
            {
                logger.Error($"Script function '{hook.FunctionId}' for trigger '{triggerName}' doesn't exist.");
                return;
            }

            if(arg is null)
                initFunction.Invoke(entity);
            else
                initFunction.Invoke(entity, arg);
        }

        #endregion Public Methods
    }
}
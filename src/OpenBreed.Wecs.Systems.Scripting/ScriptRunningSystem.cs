using OpenBreed.Common.Interface.Logging;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Scripting;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Scripting
{
    public class ScriptRunningSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IScriptMan scriptMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal ScriptRunningSystem(
            IScriptMan scriptMan,
            ILogger logger)
        {
            this.scriptMan = scriptMan;
            this.logger = logger;

            RequireEntityWith<ScriptRunnerComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        private void TryInvoke(Entity entity, string triggerName)
        {
            var component = entity.TryGet<ScriptRunnerComponent>();

            var hook = component.SystemHooks.FirstOrDefault(item => item.TriggerName == triggerName);

            if (hook is null)
                return;

            var initFunction = scriptMan.GetFunction(hook.FunctionId);

            if (initFunction is null)
            {
                logger.Error($"Script function '{hook.FunctionId}' for trigger '{triggerName}' doesn't exist.");
                return;
            }

            initFunction.Invoke(entity);
        }

        protected override void OnAddEntity(Entity entity)
        {
            base.OnAddEntity(entity);

            TryInvoke(entity, "onInit");
        }

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            TryInvoke(entity, "onUpdate");

            var sc = entity.Get<ScriptRunnerComponent>();

            for (int i = 0; i < sc.Runs.Count; i++)
            {
                var scriptAction = sc.Runs[i];

                if (scriptAction.TriggerName != "OnWorldUpdate")
                    continue;

                IScriptFunc func = null;

                if(scriptAction.ScriptFunction is not null)
                {
                    func = scriptMan.GetTableFunction($"{scriptAction.ScriptId}.{scriptAction.ScriptFunction}");

                    if (func is not null)
                        func.Invoke(entity);

                    continue;
                }

                func = scriptMan.GetFunction(scriptAction.ScriptId);

                if (func is not null)
                    func.Invoke(entity);
            }
        }

        #endregion Protected Methods
    }
}
using OpenBreed.Common.Interface;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Wecs.Components.Scripting
{
    public interface IScriptComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string ScriptId { get; }

        #endregion Public Properties
    }

    [DebuggerDisplay("{TriggerName} => {FunctionId}")]
    public class ScriptHook
    {
        public ScriptHook(
            string triggerName,
            string functionId)
        {
            TriggerName = triggerName;
            FunctionId = functionId;
        }

        public string TriggerName { get; }
        public string FunctionId { get; }
    }

    public class ScriptComponent : IEntityComponent
    {
        #region Public Constructors

        public ScriptComponent(
            string scriptId,
            ScriptHook[] scriptHooks)
        {
            ScriptId = scriptId;
            SystemHooks = scriptHooks;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ScriptId { get; }

        public ScriptHook[] SystemHooks { get; }

        #endregion Public Properties
    }

    public sealed class ScriptComponentFactory : ComponentFactoryBase<IScriptComponentTemplate>
    {
        private readonly IScriptMan scriptMan;
        private readonly IDataLoaderFactory dataLoaderFactory;
        #region Internal Constructors

        public ScriptComponentFactory(
            IScriptMan scriptMan,
            IDataLoaderFactory dataLoaderFactory)
        {
            this.scriptMan = scriptMan;
            this.dataLoaderFactory = dataLoaderFactory;
        }

        #endregion Internal Constructors

        #region Protected Methods

        private ScriptHook[] GetSystemHooks(string scriptId, Dictionary<object, object> engineHooks)
        {
            var systemHooks = new List<ScriptHook>(); 

            foreach (var hook in engineHooks)
            {
                if (hook.Key is not string hookName)
                    continue;

                if(hook.Value is not IScriptFunc hookFunc)
                    continue;

                var functionId = $"{scriptId}.{hook.Key}";

                if(!scriptMan.FunctionExists(functionId))
                    scriptMan.RegisterFunction(functionId, hookFunc);

                systemHooks.Add(new ScriptHook(hookName, functionId));
            }

            return systemHooks.ToArray();
        }

        protected override IEntityComponent Create(IScriptComponentTemplate template)
        {
            var scriptLoader = dataLoaderFactory.GetLoader<IScriptDataLoader>();

            var systemHooks = Array.Empty<ScriptHook>();

            if (!string.IsNullOrEmpty(template.ScriptId))
            {
                var scriptFunc = scriptLoader.Load(template.ScriptId);
                var result =  scriptFunc.Invoke();

                if(result is object[] array)
                {
                    if(array[0] is Dictionary<object, object> entityHooks)
                    {
                        foreach (var hook in entityHooks)
                        {
                            if(hook.Key is string hookName)
                            {
                                if(hookName == "systemHooks")
                                {
                                    if (hook.Value is Dictionary<object, object> engineHooks)
                                        systemHooks = GetSystemHooks(template.ScriptId, engineHooks);
                                }
                            }
                        }
                    }
                }
            }

            return new ScriptComponent(
                template.ScriptId,
                systemHooks);
        }

        #endregion Protected Methods
    }
}

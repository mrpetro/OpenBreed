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
    public interface IScriptRunTemplate
    {
        string ScriptId { get; set; }
        string TriggerName { get; set; }
        string ScriptFunction { get; set; }
    }

    public interface IScriptRunnerComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string ScriptId { get; }

        IEnumerable<IScriptRunTemplate> Runs { get; }

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

    public class ScriptRun
    {
        public ScriptRun(
            string scriptId,
            string triggerName,
            string scriptFunction)
        {
            ScriptId = scriptId;
            TriggerName = triggerName;
            ScriptFunction = scriptFunction;
        }

        public string ScriptId { get; }
        public string TriggerName { get; }
        public string ScriptFunction { get; }
    }



    public class ScriptRunnerComponent : IEntityComponent
    {
        #region Public Constructors

        public ScriptRunnerComponent(
            string scriptId,
            ScriptHook[] scriptHooks,
            List<ScriptRun> runs)
        {
            ScriptId = scriptId;
            SystemHooks = scriptHooks;
            Runs = runs;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ScriptId { get; }

        public ScriptHook[] SystemHooks { get; }

        public List<ScriptRun> Runs { get; }

        #endregion Public Properties
    }

    public sealed class ScriptRunnerComponentFactory : ComponentFactoryBase<IScriptRunnerComponentTemplate>
    {
        private readonly IScriptMan scriptMan;
        private readonly IDataLoaderFactory dataLoaderFactory;
        #region Internal Constructors

        public ScriptRunnerComponentFactory(
            IScriptMan scriptMan,
            IDataLoaderFactory dataLoaderFactory)
        {
            this.scriptMan = scriptMan;
            this.dataLoaderFactory = dataLoaderFactory;
        }

        #endregion Internal Constructors

        #region Protected Methods

        public void RegisterEngineHook<TSystem>(string hookName, Func<TSystem, Action<Entity>> func) where TSystem : ISystem
        {
        }

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

        protected override IEntityComponent Create(IScriptRunnerComponentTemplate template)
        {
            var scriptLoader = dataLoaderFactory.GetLoader<IScriptDataLoader>();

            var systemHooks = Array.Empty<ScriptHook>();

            if (template.ScriptId is not null)
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


                //var result = scriptFunc.Invoke<EntityValueWrapper, EntityScript>();
                //var s = scriptMan.GetFunction($"EntityTypes.{metadata.Name}");

                //var result = s.Invoke(entity);
                //scriptMan.RegisterFunction(template.ScriptId, scriptFunc);
            }

            var runs = new List<ScriptRun>();

            foreach (var runTemplate in template.Runs)
            {
                if (!scriptMan.FunctionExists(runTemplate.ScriptId))
                {
                    var func = scriptLoader.Load(runTemplate.ScriptId);

                    if(runTemplate.ScriptFunction is not null)
                    {
                        func.Invoke();
                    }
                    else
                        scriptMan.RegisterFunction(runTemplate.ScriptId, func);
                }

                runs.Add(new ScriptRun(
                    runTemplate.ScriptId,
                    runTemplate.TriggerName,
                    runTemplate.ScriptFunction));
            }

            return new ScriptRunnerComponent(
                template.ScriptId,
                systemHooks,
                runs);
        }

        #endregion Protected Methods
    }
}

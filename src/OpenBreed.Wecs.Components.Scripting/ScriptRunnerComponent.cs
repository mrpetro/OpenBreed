using OpenBreed.Common.Interface;
using OpenBreed.Scripting.Interface;
using System;
using System.Collections.Generic;
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

        IEnumerable<IScriptRunTemplate> Runs { get; }

        #endregion Public Properties
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

        public ScriptRunnerComponent(List<ScriptRun> runs)
        {
            Runs = runs;
        }

        #endregion Public Constructors

        #region Public Properties

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

        protected override IEntityComponent Create(IScriptRunnerComponentTemplate template)
        {
            var scriptLoader = dataLoaderFactory.GetLoader<IScriptDataLoader>();

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

            return new ScriptRunnerComponent(runs);
        }

        #endregion Protected Methods
    }
}

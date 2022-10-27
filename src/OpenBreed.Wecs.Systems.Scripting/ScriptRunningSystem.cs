using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Scripting;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Wecs.Systems.Scripting
{
    public class ScriptRunningSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IScriptMan scriptMan;

        #endregion Private Fields

        #region Internal Constructors

        internal ScriptRunningSystem(IScriptMan scriptMan)
        {
            this.scriptMan = scriptMan;

            RequireEntityWith<ScriptRunnerComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            base.OnAddEntity(entity);


            var metadata = entity.TryGet<MetadataComponent>();

            var initFunc = scriptMan.GetTableFunction($"EntityTypes.{metadata.Name}.Initialize");
            initFunc.Invoke(entity);
        }

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
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
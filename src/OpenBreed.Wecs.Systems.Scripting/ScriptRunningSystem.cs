using OpenBreed.Scripting.Interface;
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

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            //var sc = entity.Get<ScriptRunnerComponent>();

            //for (int i = 0; i < sc.Runs.Count; i++)
            //{
            //    var script = sc.Runs[i];

            //    if(script.ToRun)
            //        scriptMan.TryInvokeFunction()

            //}
        }

        #endregion Protected Methods
    }
}
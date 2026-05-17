using OpenBreed.Common.Interface;
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Scripting.Lua;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Wecs.Scripting.Components
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
}

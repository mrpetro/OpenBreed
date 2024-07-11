using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Scripting;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using System;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Scripting
{
    [RequireEntityWith(typeof(ScriptComponent))]
    public class ScriptRunningSystem : UpdatableMatchingSystemBase<ScriptRunningSystem>
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
        }

        #endregion Internal Constructors

        #region Protected Methods

        public override void AddEntity(IEntity entity)
        {
            base.AddEntity(entity);

            entity.TryInvoke(scriptMan, logger, "OnInit");
        }

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            entity.TryInvoke(scriptMan, logger, "OnUpdate");
        }

        #endregion Protected Methods
    }
}
using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Scripting;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
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

        #region Public Constructors

        public ScriptRunningSystem(
            IScriptMan scriptMan,
            ILogger logger)
        {
            this.scriptMan = scriptMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void OnAddEntity(IWorld world, IEntity entity)
        {
            base.OnAddEntity(world, entity);

            entity.TryInvoke(scriptMan, logger, "OnInit");
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            entity.TryInvoke(scriptMan, logger, "OnUpdate");
        }

        #endregion Protected Methods
    }
}
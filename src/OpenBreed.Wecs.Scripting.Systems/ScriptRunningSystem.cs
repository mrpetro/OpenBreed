using Microsoft.Extensions.Logging;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Scripting.Components;
using OpenBreed.Wecs.Core.Systems;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using System;
using System.Linq;

namespace OpenBreed.Wecs.Scripting.Systems
{
    [RequireEntityWith(typeof(ScriptComponent))]
    public class ScriptRunningSystem : UpdatableMatchingSystemBase, IOnAddEntitySystem
    {
        #region Private Fields

        private readonly IScriptMan scriptMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public ScriptRunningSystem(
            IWorldMan worldMan,
            IScriptMan scriptMan,
            ILogger logger) : base(worldMan)
        {
            this.scriptMan = scriptMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnAddEntity(IWorld world, IEntity entity)
        {
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
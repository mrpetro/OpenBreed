﻿using OpenBreed.Common.Interface.Logging;
using OpenBreed.Scripting.Interface;
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
    public class ScriptRunningSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IScriptMan scriptMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal ScriptRunningSystem(
            IWorld world,
            IScriptMan scriptMan,
            ILogger logger) :
            base(world)
        {
            this.scriptMan = scriptMan;
            this.logger = logger;

            RequireEntityWith<ScriptComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void OnAddEntity(IEntity entity)
        {
            base.OnAddEntity(entity);

            entity.TryInvoke(scriptMan, logger, "onInit");
        }

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            entity.TryInvoke(scriptMan, logger, "onUpdate");
        }

        #endregion Protected Methods
    }
}
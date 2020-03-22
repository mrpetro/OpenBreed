using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Systems
{
    public class StateMachineSystem : WorldSystem, ICommandExecutor
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();

        private readonly CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        public StateMachineSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);
            Require<FsmComponent>();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods
    }
}
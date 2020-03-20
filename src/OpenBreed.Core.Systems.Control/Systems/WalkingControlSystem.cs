using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;

using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Animation.Systems.Control.Commands;
using OpenBreed.Core.Systems.Control.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Systems;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Systems
{
    public class WalkingControlSystem : WorldSystem, IUpdatableSystem, ICommandExecutor
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        internal WalkingControlSystem(WalkingControlSystemBuilder builder) : base(builder.core)
        {
            cmdHandler = new CommandHandler(this);

            Require<IControlComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(WalkingControlCommand.TYPE, cmdHandler);
            World.RegisterHandler(AttackControlCommand.TYPE, cmdHandler);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public void Update(float dt)
        {
            cmdHandler.ExecuteEnqueued();
        }

        public override bool ExecuteCommand(object sender, ICommand cmd)
        {
            switch (cmd.Type)
            {
                case WalkingControlCommand.TYPE:
                    return HandleWalkingControlCommand(sender, (WalkingControlCommand)cmd);
                case AttackControlCommand.TYPE:
                    return HandleAttackControlCommand(sender, (AttackControlCommand)cmd);
                default:
                    return false;
            }
        }

        #endregion Public Methods

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

        #region Private Methods

        private bool HandleAttackControlCommand(object sender, AttackControlCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);


            var control = entity.GetComponent<AttackControl>();

            if (control.AttackPrimary != cmd.Primary)
            {
                control.AttackPrimary = cmd.Primary;
                entity.RaiseEvent(new ControlFireChangedEvenrArgs(control.AttackPrimary));
            }

            return true;
        }

        private bool HandleWalkingControlCommand(object sender, WalkingControlCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);

            var control = entity.GetComponent<WalkingControl>();

            if (control.Direction != cmd.Direction)
            {
                control.Direction = cmd.Direction;
                entity.RaiseEvent(new ControlDirectionChangedEventArgs(control.Direction));
            }

            return true;
        }

        #endregion Private Methods
    }
}
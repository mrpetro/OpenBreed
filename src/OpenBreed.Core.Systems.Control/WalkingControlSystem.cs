using OpenBreed.Core;
using OpenBreed.Components.Common;
using OpenBreed.Components.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Systems.Control.Builders;
using OpenBreed.Core.Managers;
using OpenBreed.Systems.Core;
using OpenBreed.Ecsw.Systems;
using OpenBreed.Systems.Control.Commands;
using OpenBreed.Systems.Control.Events;
using OpenBreed.Ecsw.Entities;
using OpenBreed.Ecsw;

namespace OpenBreed.Systems.Control
{
    public class WalkingControlSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Constructors

        internal WalkingControlSystem(WalkingControlSystemBuilder builder) : base(builder.core)
        {
            Require<IControlComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commands)
        {
            commands.Register<WalkingControlCommand>(HandleWalkingControlCommand);
            commands.Register<AttackControlCommand>(HandleAttackControlCommand);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public void Update(float dt)
        {
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private static bool HandleAttackControlCommand(ICore core, AttackControlCommand cmd)
        {
            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);

            var control = entity.Get<AttackControl>();

            if (control.AttackPrimary != cmd.Primary)
            {
                control.AttackPrimary = cmd.Primary;
                entity.RaiseEvent(new ControlFireChangedEvenrArgs(control.AttackPrimary));
            }

            return true;
        }

        private static bool HandleWalkingControlCommand(ICore core, WalkingControlCommand cmd)
        {
            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);

            var control = entity.Get<WalkingControl>();

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
using OpenBreed.Core;
using OpenBreed.Core.Components;
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
using OpenBreed.Core.Managers;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Systems
{
    public class WalkingControlSystem : WorldSystem, IUpdatableSystem
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

        public static void RegisterHandlers(CommandsMan commands)
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
            var entity = core.Entities.GetById(cmd.EntityId);

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
            var entity = core.Entities.GetById(cmd.EntityId);

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
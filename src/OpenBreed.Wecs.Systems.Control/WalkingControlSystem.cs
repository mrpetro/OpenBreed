using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Control.Commands;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;

namespace OpenBreed.Wecs.Systems.Control
{
    public class WalkingControlSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Constructors

        internal WalkingControlSystem()
        {
            Require<IControlComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commands)
        {
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

            var control = entity.Get<AttackControlComponent>();

            if (control.AttackPrimary != cmd.Primary)
            {
                control.AttackPrimary = cmd.Primary;
                entity.RaiseEvent(new ControlFireChangedEvenrArgs(control.AttackPrimary));
            }

            return true;
        }

        #endregion Private Methods
    }
}
using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Ecsw.Systems;
using OpenBreed.Ecsw.Entities;
using OpenBreed.Ecsw;
using OpenBreed.Fsm;

namespace OpenBreed.Systems.Core
{
    public class StateMachineSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Constructors

        public StateMachineSystem(ICore core) : base(core)
        {
            Require<FsmComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commands)
        {
            commands.Register<SetStateCommand>(HandleSetStateCommand);
        }

        public void Update(float dt)
        {
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);

            InitializeComponent(entity);
            //World.Subscribe<EntityAddedEventArgs>(OnEntityEnteredWorld);
            //entity.Subscribe<EntityEnteredWorldEventArgs>(OnEntityEnteredWorld);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            //World.Unsubscribe<EntityAddedEventArgs>(OnEntityEnteredWorld);
            //entity.Unsubscribe<EntityEnteredWorldEventArgs>(OnEntityEnteredWorld);

            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeComponent(Entity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            foreach (var state in fsmComponent.States)
                Core.GetManager<IFsmMan>().EnterState(entity, state, 0);
        }

        private void DeinitializeComponent(Entity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            foreach (var state in fsmComponent.States)
                Core.GetManager<IFsmMan>().EnterState(entity, state, 0);
        }

        private static bool HandleSetStateCommand(ICore core, SetStateCommand cmd)
        {
            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);

            var fsmComponent = entity.Get<FsmComponent>();

            if (fsmComponent == null)
            {
                core.Logging.Warning($"Entity '{cmd.EntityId}' has missing FSM component.");
                return false;
            }

            var fsm = core.GetManager<IFsmMan>().GetById(cmd.FsmId);

            var fsmData = fsmComponent.States.FirstOrDefault(item => item.FsmId == cmd.FsmId);

            if (fsmData == null)
            {
                core.Logging.Warning($"Entity '{cmd.EntityId}' has missing data for FSM '{fsm.Name}'.");
                return false;
            }

            core.GetManager<IFsmMan>().LeaveState(entity, fsmData, cmd.ImpulseId);
            var nextStateId = fsm.GetNextStateId(fsmData.StateId, cmd.ImpulseId);

            if (nextStateId == -1)
            {
                var fromStateName = fsm.GetStateName(fsmData.StateId);
                var impulseName = fsm.GetImpulseName(cmd.ImpulseId);

                core.Logging.Warning($"Entity '{cmd.EntityId}' has missing FSM transition from state '{fromStateName}' using impulse '{impulseName}'.");
                return false;
            }

            fsmData.StateId = nextStateId;
            core.GetManager<IFsmMan>().EnterState(entity, fsmData, cmd.ImpulseId);

            return true;
        }

        #endregion Private Methods
    }
}
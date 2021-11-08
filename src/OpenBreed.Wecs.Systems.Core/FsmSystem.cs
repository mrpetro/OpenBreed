using OpenBreed.Common.Logging;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Core
{
    public class FsmSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;
        private readonly IFsmMan fsmMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public FsmSystem(IEntityMan entityMan, IFsmMan fsmMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.fsmMan = fsmMan;
            this.logger = logger;

            RequireEntityWith<FsmComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(float dt)
        {
            ExecuteCommands();

            foreach (var entity in entities)
            {
                var fsmCmp = entity.Get<FsmComponent>();

                var toUpdate = fsmCmp.States.Where(state => state.ImpulseId != MachineState.NO_IMPULSE);

                foreach (var machineState in toUpdate)
                {
                    UpdateWithImpulse(entity, machineState);
                }
            }
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            ExecuteCommands();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);

            InitializeComponent(entity);
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

        private void InitializeComponent(Entity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            foreach (var state in fsmComponent.States)
                fsmMan.EnterState(entity, state, 0);
        }

        private void DeinitializeComponent(Entity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            foreach (var state in fsmComponent.States)
                fsmMan.EnterState(entity, state, 0);
        }

        private void UpdateWithImpulse(Entity entity, MachineState machineState)
        {
            var impulseId = machineState.ImpulseId;

            try
            {
                var fsm = fsmMan.GetById(machineState.FsmId);

                fsmMan.LeaveState(entity, machineState, impulseId);
                var nextStateId = fsm.GetNextStateId(machineState.StateId, impulseId);

                if (nextStateId == -1)
                {
                    var fromStateName = fsm.GetStateName(machineState.StateId);
                    var impulseName = fsm.GetImpulseName(impulseId);

                    logger.Warning($"Entity '{entity.Id}' has missing FSM transition from state '{fromStateName}' using impulse '{impulseName}'.");
                    return;
                }

                machineState.StateId = nextStateId;
                fsmMan.EnterState(entity, machineState, impulseId);
            }
            finally
            {
                //Check if impulse was changed already in EnterState
                if (impulseId == machineState.ImpulseId)
                    machineState.ImpulseId = MachineState.NO_IMPULSE;
            }
        }

        #endregion Private Methods
    }
}
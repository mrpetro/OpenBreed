using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Core
{
    public class FsmSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public FsmSystem(IFsmMan fsmMan, ILogger logger)
        {
            this.fsmMan = fsmMan;
            this.logger = logger;

            RequireEntityWith<FsmComponent>();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            var fsmCmp = entity.Get<FsmComponent>();

            var toUpdate = fsmCmp.States.Where(state => state.ImpulseId != MachineState.NO_IMPULSE);

            foreach (var machineState in toUpdate)
            {
                UpdateEntityWithImpulse(entity, machineState);
            }
        }

        protected override void OnAddEntity(Entity entity)
        {
            base.OnAddEntity(entity);

            InitializeComponent(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            DeinitializeComponent(entity);

            base.OnRemoveEntity(entity);
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

        private void UpdateEntityWithImpulse(Entity entity, MachineState machineState)
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
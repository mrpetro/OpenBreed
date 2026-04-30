using Microsoft.Extensions.Logging;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Core.Systems.Categories;
using System.Linq;

namespace OpenBreed.Wecs.Core.Systems
{
    [RequireEntityWith(typeof(FsmComponent))]
    [SystemCategory(CommonCategories.General)]
    public class FsmSystem : UpdatableMatchingSystemBase, IEventSystem<EntityEnteredEvent>, IEventSystem<EntityLeftEvent>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        private readonly IFsmMan fsmMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public FsmSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IFsmMan fsmMan,
            ILogger logger) : base(worldMan)
        {
            this.entityMan = entityMan;
            this.fsmMan = fsmMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(
            [TargetWorldAsSourceFilter]
            EntityEnteredEvent e, IWorld world)
        {
            var entity = entityMan.GetById(e.EntityId);

            InitializeComponent(entity);
        }

        public void OnEvent(
            [TargetWorldAsSourceFilter]
            EntityLeftEvent e, IWorld world)
        {
            var entity = entityMan.GetById(e.EntityId);

            DeinitializeComponent(entity);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var fsmCmp = entity.Get<FsmComponent>();

            var toUpdate = fsmCmp.States.Where(state => state.ImpulseId != MachineState.NO_IMPULSE);

            foreach (var machineState in toUpdate)
            {
                UpdateEntityWithImpulse(entity, machineState);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeComponent(IEntity entity)
        {
            var fsmComponent = entity.TryGet<FsmComponent>();

            if (fsmComponent is null)
            {
                return;
            }

            foreach (var state in fsmComponent.States)
                fsmMan.EnterState(entity, state, 0);
        }

        private void DeinitializeComponent(IEntity entity)
        {
            var fsmComponent = entity.TryGet<FsmComponent>();

            if (fsmComponent is null)
            {
                return;
            }

            foreach (var state in fsmComponent.States)
                fsmMan.EnterState(entity, state, 0);
        }

        private void UpdateEntityWithImpulse(IEntity entity, MachineState machineState)
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

                    logger.LogWarning("Entity '{0}' has missing FSM transition from state '{1}' using impulse '{2}'.", entity.Id, fromStateName, impulseName);
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
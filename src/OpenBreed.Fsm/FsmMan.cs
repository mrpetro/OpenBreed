using OpenBreed.Common.Tools.Collections;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Fsm
{
    public class FsmMan : IFsmMan
    {
        #region Private Fields

        private IdMap<IStateMachine> list = new IdMap<IStateMachine>();

        #endregion Private Fields

        #region Public Constructors

        public FsmMan()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IStateMachine GetById(int id)
        {
            if (list.TryGetValue(id, out IStateMachine fsm))
                return fsm;
            else
                throw new InvalidOperationException($"State machine with ID '{id}' not found.");
        }

        public IStateMachine GetByName(string name)
        {
            return list.Items.FirstOrDefault(item => item.Name == name);
        }

        public string GetStateName(int fsmId, int stateId)
        {
            var fsm = GetById(fsmId);
            return fsm.GetStateName(stateId);
        }

        public string GetStateName(MachineState state)
        {
            return GetStateName(state.FsmId, state.StateId);
        }

        public IEnumerable<string> GetStateNames(IEntity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();
            return fsmComponent.States.Select(item => GetStateName(item));
        }

        public IStateMachine<TState, TImpulse> Create<TState, TImpulse>(string name) where TState : Enum where TImpulse : Enum
        {
            var newFsm = new StateMachine<TState, TImpulse>(name);
            newFsm.Id = list.Add(newFsm);
            return newFsm;
        }

        public void EnterState(IEntity entity, MachineState state, int withImpulseId)
        {
            list[state.FsmId].EnterState(entity, state.StateId, withImpulseId);
        }

        public void LeaveState(IEntity entity, MachineState state, int withImpulseId)
        {
            list[state.FsmId].LeaveState(entity, state.StateId, withImpulseId);
        }

        #endregion Public Methods
    }
}
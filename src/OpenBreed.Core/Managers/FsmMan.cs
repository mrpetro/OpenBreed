using OpenBreed.Core.Collections;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Managers
{
    public class FsmMan
    {
        #region Private Fields

        private IdMap<IStateMachine> list = new IdMap<IStateMachine>();

        #endregion Private Fields

        #region Public Constructors

        public FsmMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

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

        public IEnumerable<string> GetStateNames(Entity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();
            return fsmComponent.States.Select(item => GetStateName(item));
        }

        public StateMachine<TState, TImpulse> Create<TState, TImpulse>(string name) where TState : Enum where TImpulse : Enum
        {
            var newFsm = new StateMachine<TState, TImpulse>(Core, name);
            newFsm.Id = list.Add(newFsm);
            return newFsm;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void EnterState(Entity entity, MachineState state, int withImpulseId)
        {
            list[state.FsmId].EnterState(entity, state.StateId, withImpulseId);
        }

        internal void LeaveState(Entity entity, MachineState state, int withImpulseId)
        {
            list[state.FsmId].LeaveState(entity, state.StateId, withImpulseId);
        }

        #endregion Internal Methods
    }
}
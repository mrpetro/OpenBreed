using OpenBreed.Core.Collections;
using OpenBreed.Core.Common.Components;
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

        private IdMap<IStateMachineEx> list = new IdMap<IStateMachineEx>();

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

        public IStateMachineEx GetById(int id)
        {
            if (list.TryGetValue(id, out IStateMachineEx fsm))
                return fsm;
            else
                throw new InvalidOperationException($"State machine with ID '{id}' not found.");
        }

        public IStateMachineEx GetByName(string name)
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
            var fsmComponent = entity.GetComponent<FsmComponent>();
            return fsmComponent.States.Select(item => GetStateName(item));
        }

        public StateMachineEx<TState, TImpulse> Create<TState, TImpulse>(string name) where TState : Enum where TImpulse : Enum
        {
            var newFsm = new StateMachineEx<TState, TImpulse>(name);
            newFsm.Id = list.Add(newFsm);
            return newFsm;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void EnterState(IEntity entity, MachineState state)
        {
            list[state.FsmId].EnterState(entity, state.StateId);
        }

        internal void LeaveState(IEntity entity, MachineState state)
        {
            list[state.FsmId].LeaveState(entity, state.StateId);
        }

        #endregion Internal Methods
    }
}
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.States
{
    public class StateMachine
    {
        #region Private Fields

        private IEntity entity;
        private Dictionary<string, IState> states = new Dictionary<string, IState>();
        private IState currentState = null;
        private string initialStateId;

        #endregion Private Fields

        #region Public Constructors

        public StateMachine(IEntity entity)
        {
            this.entity = entity;

            entity.PerformDelegate = Perform;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddState(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (string.IsNullOrWhiteSpace(state.Id))
                throw new ArgumentNullException(nameof(state.Id));

            states.Add(state.Id, state);
        }

        public void Initialize()
        {
            foreach (var state in states)
                state.Value.Initialize(entity);

            if (currentState != null)
                throw new InvalidOperationException($"Initial state already set to '{currentState.Id}'");

            currentState = states[initialStateId];
            //Console.WriteLine($"Entering state '{currentState.Id}'");
            currentState.EnterState();
        }

        public void Perform(string actionName, params object[] arguments)
        {
            var nextStateName = currentState.Process(actionName, arguments);

            if (nextStateName == null)
                return;

            if (nextStateName != currentState.Id)
                ChangeState(nextStateName);
        }

        public void SetInitialState(string stateId, params object[] arguments)
        {
            initialStateId = stateId;
        }

        #endregion Public Methods

        #region Private Methods

        private void ChangeState(string nextStateId)
        {
            //Console.WriteLine($"Leaving state '{currentState.Id}'");
            currentState.LeaveState();
            currentState = states[nextStateId];
            //Console.WriteLine($"Entering state '{currentState.Id}'");
            currentState.EnterState();
        }

        #endregion Private Methods
    }
}
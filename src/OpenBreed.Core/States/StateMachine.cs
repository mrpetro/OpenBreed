using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.States
{
    public class StateMachine : IEntityComponent
    {
        #region Private Fields

        private Dictionary<string, IState> states = new Dictionary<string, IState>();
        private IState currentState = null;
        private string initialStateId;

        public Type SystemType { get { return null; } }

        #endregion Private Fields

        #region Public Methods

        public void AddState(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (string.IsNullOrWhiteSpace(state.Id))
                throw new ArgumentNullException(nameof(state.Id));

            states.Add(state.Id, state);
        }

        public void Initialize(IEntity entity)
        {
            foreach (var state in states)
                state.Value.Initialize(entity);

            if (currentState != null)
                throw new InvalidOperationException($"Initial state already set to '{currentState.Id}'");

            currentState = states[initialStateId];
            //Console.WriteLine($"Entering state '{currentState.Id}'");
            currentState.EnterState();
        }

        public void Deinitialize(IEntity entity)
        {

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

        #endregion Private Methods
    }
}
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.States
{
    public class StateMachine
    {
        #region Private Fields

        private Dictionary<string, IState> states = new Dictionary<string, IState>();
        private IState currentState = null;

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

        public void Update(float dt)
        {
            var nextStateName = currentState.Update(dt);

            if (nextStateName == null)
                return;

            if (nextStateName != currentState.Id)
                ChangeState(nextStateName);
        }

        #endregion Public Methods

        #region Private Methods

        private void ChangeState(string nextStateId)
        {
            currentState.LeaveState();
            currentState = states[nextStateId];
            currentState.EnterState();
        }

        #endregion Private Methods
    }
}
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.States
{
    public class StateMachine : IMsgHandler
    {
        #region Private Fields

        private Dictionary<string, IState> states = new Dictionary<string, IState>();
        private IState currentState = null;

        #endregion Private Fields

        #region Public Constructors

        internal StateMachine(string name, IEntity entity)
        {
            Name = name;
            Entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }

        public IEntity Entity { get; }

        #endregion Public Properties

        #region Public Methods

        public void AddState(IState state)
        {
            Debug.Assert(state != null, "State must not be null");

            if (string.IsNullOrWhiteSpace(state.Id))
                throw new ArgumentNullException(nameof(state.Id));

            states.Add(state.Id, state);
        }

        internal void Initialize()
        {
            if (currentState == null)
                throw new InvalidOperationException("Initial state not set");

            foreach (var state in states)
                state.Value.Initialize(Entity);

            currentState.EnterState();
        }

        internal void Deinitialize()
        {
            currentState.LeaveState();

            //foreach (var state in states)
            //    state.Value.Deinitialize(Entity);
        }

        /// <summary>
        /// Set particular initial state for this state machine
        /// </summary>
        /// <param name="initialStateId">Initial state id</param>
        public void SetInitialState(string initialStateId)
        {
            if (currentState != null)
                throw new InvalidOperationException($"Initial state already set to '{currentState.Id}'");

            currentState = states[initialStateId];
        }

        public bool HandleMsg(object sender, IMsg message)
        {
            switch (message.Type)
            {
                case StateChangeMsg.TYPE:
                    return HandleStateChangeMsg(sender, (StateChangeMsg)message);

                default:
                    return false;
            }
        }

        public void Perform(string actionName, params object[] arguments)
        {
            var nextStateName = currentState.Process(actionName, arguments);

            if (nextStateName == null)
                return;

            if (nextStateName != currentState.Id)
                ChangeState(nextStateName, arguments);
        }

        #endregion Public Methods

        #region Private Methods

        private bool HandleStateChangeMsg(object sender, StateChangeMsg message)
        {
            Perform(message.StateId, message.Args);
            return true;
        }

        private void ChangeState(string nextStateId, params object[] arguments)
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
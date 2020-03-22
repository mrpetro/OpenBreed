using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.States
{
    public interface IStateMachine : IMsgHandler
    {
        string Name { get; }
        string CurrentStateName { get; }
        void Initialize();
        void Deinitialize();
    }


    public class StateMachine<T> : IStateMachine where T : struct, IConvertible
    {
        #region Private Fields

        private Dictionary<T, IState<T>> states = new Dictionary<T, IState<T>>();
        private IState<T> currentState;

        #endregion Private Fields

        #region Internal Constructors

        internal StateMachine(IEntity entity)
        {
            Entity = entity;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Name => typeof(T).Name;

        public string CurrentStateName => currentState.Id.ToString();

        public IEntity Entity { get; }

        public T CurrentStateId { get { return currentState.Id; } }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{typeof(T)}({CurrentStateId})";
        }

        public void AddState(IState<T> state)
        {
            Debug.Assert(state != null, "State must not be null");

            if (!Enum.IsDefined(typeof(T), state.Id))
                throw new InvalidOperationException($"State '{state.Id}' not defined in this FSM.");

            states.Add(state.Id, state);
        }

        /// <summary>
        /// Set particular initial state for this state machine
        /// </summary>
        /// <param name="initialStateId">Initial state id</param>
        public void SetInitialState(T initialStateId)
        {
            if (currentState != null)
                throw new InvalidOperationException($"Initial state already set to '{currentState.Id}'");

            currentState = states[initialStateId];
        }

        public bool Handle(object sender, IMsg cmd)
        {
            switch (cmd.Type)
            {
                case EntitySetStateCommand.TYPE:
                    return HandleStateChangeMsg(sender, (EntitySetStateCommand)cmd);

                default:
                    return false;
            }
        }

        public void Perform(string actionName, params object[] arguments)
        {
            var nextStateName = currentState.Process(actionName, arguments);

            if (!Equals(nextStateName,currentState.Id))
                ChangeState(nextStateName, arguments);
        }

        #endregion Public Methods

        #region Internal Methods

        public void Initialize()
        {
            if (currentState == null)
                throw new InvalidOperationException("Initial state not set");

            foreach (var state in states)
                state.Value.Initialize(Entity);

            currentState.EnterState();
        }

        public void Deinitialize()
        {
            currentState.LeaveState();

            //foreach (var state in states)
            //    state.Value.Deinitialize(Entity);
        }

        #endregion Internal Methods

        #region Private Methods

        private bool HandleStateChangeMsg(object sender, EntitySetStateCommand message)
        {
            Perform(message.StateId);
            return true;
        }

        private void ChangeState(T nextStateId, params object[] arguments)
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
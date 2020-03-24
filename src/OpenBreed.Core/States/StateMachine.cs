using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Core.States
{
    public interface IStateMachine : IMsgHandler
    {

        #region Public Properties

        string Name { get; }
        string CurrentStateName { get; }

        #endregion Public Properties

        #region Public Methods

        void Initialize();

        void Deinitialize();

        #endregion Public Methods

    }

    public class StateMachine<TState, TImpulse> : IStateMachine where TState : struct, IConvertible where TImpulse : struct, IConvertible
    {

        #region Private Fields

        private Dictionary<TState, IState<TState, TImpulse>> states = new Dictionary<TState, IState<TState, TImpulse>>();
        private IState<TState, TImpulse> currentState;

        private Dictionary<TState, Dictionary<TImpulse, TState>> transitions = new Dictionary<TState, Dictionary<TImpulse, TState>>();

        private Dictionary<TState, Action> onEntryActions = new Dictionary<TState, Action>();
        private Dictionary<TState, Action> onLeaveActions = new Dictionary<TState, Action>();

        #endregion Private Fields

        #region Internal Constructors

        internal StateMachine(IEntity entity)
        {
            Entity = entity;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Name => typeof(TState).Name;

        public string CurrentStateName => currentState.Id.ToString();

        public IEntity Entity { get; }

        public TState CurrentStateId { get { return currentState.Id; } }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{typeof(TState)}({CurrentStateId})";
        }
        public void SetStateEntry(TState state, Action action)
        {
            onEntryActions[state] = action;
        }

        public void SetStateLeave(TState state, Action action)
        {
            onLeaveActions[state] = action;
        }

        public void AddState(IState<TState, TImpulse> state)
        {
            Debug.Assert(state != null, "State must not be null");

            if (!Enum.IsDefined(typeof(TState), state.Id))
                throw new InvalidOperationException($"State '{state.Id}' not defined in this FSM.");

            states.Add(state.Id, state);
        }

        /// <summary>
        /// Set particular initial state for this state machine
        /// </summary>
        /// <param name="initialStateId">Initial state id</param>
        public void SetInitialState(TState initialStateId)
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

        public void Perform(TImpulse impulse, params object[] arguments)
        {
            if (transitions.Any())
            {
                Dictionary<TImpulse, TState> transition;
                if (transitions.TryGetValue(CurrentStateId, out transition))
                {
                    TState toState;
                    if (transition.TryGetValue(impulse, out toState))
                    {
                        if (!Equals(toState, currentState.Id))
                            ChangeState(toState, arguments);
                    }
                    else
                        Entity.Core.Logging.Warning($"FSM: Transition from state '{CurrentStateId}' using impulse '{impulse}' not defined.");
                }
                else
                    Entity.Core.Logging.Warning($"FSM: Transition from state '{CurrentStateId}' not defined.");

                return;
            }
        }

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

        public void AddTransition(TState fromState, TImpulse impulse, TState toState)
        {
            Dictionary<TImpulse, TState> transition;

            if (!transitions.TryGetValue(fromState, out transition))
            {
                transition = new Dictionary<TImpulse, TState>();
                transitions.Add(fromState, transition);
            }

            transition[impulse] = toState;
        }


        #endregion Public Methods

        #region Private Methods

        private bool HandleStateChangeMsg(object sender, EntitySetStateCommand message)
        {
            TImpulse impulse;
            if (!Enum.TryParse(message.StateId, out impulse))
                throw new InvalidOperationException();

            Perform(impulse);
            return true;
        }

        private void ChangeState(TState nextStateId, params object[] arguments)
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
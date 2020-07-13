using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Core.States
{
    public interface IStateMachine
    {
        #region Public Properties

        int Id { get; }
        string Name { get; }

        #endregion Public Properties

        #region Public Methods

        void EnterState(Entity entity, int stateId, int withImpulseId);

        void LeaveState(Entity entity, int stateId, int withImpulseId);

        int GetNextStateId(int currentStateId, int impulseId, params object[] arguments);

        void SetInitialState(Entity entity, int initialStateId);

        string GetCurrentStateName(Entity entity);

        string GetStateName(int stateId);

        int GetStateIdByName(string stateName);

        string GetImpulseName(int impulseId);

        #endregion Public Methods
    }

    public class StateMachine<TState, TImpulse> : IStateMachine where TState : Enum where TImpulse : Enum
    {
        #region Private Fields

        private readonly Dictionary<TState, IState<TState, TImpulse>> states = new Dictionary<TState, IState<TState, TImpulse>>();
        private readonly Dictionary<TState, Dictionary<TImpulse, TState>> transitions = new Dictionary<TState, Dictionary<TImpulse, TState>>();
        private readonly Dictionary<TState, Dictionary<TImpulse, Action<ICore, int>>> onEnterActions = new Dictionary<TState, Dictionary<TImpulse, Action<ICore, int>>>();
        private readonly Dictionary<TState, Dictionary<TImpulse, Action<ICore, int>>> onLeaveActions = new Dictionary<TState, Dictionary<TImpulse, Action<ICore, int>>>();

        #endregion Private Fields

        #region Internal Constructors

        internal ICore Core { get; }

        internal StateMachine(ICore core, string name)
        {
            Core = core;
            Name = name;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int Id { get; internal set; }
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{Name}";
        }

        public void AddState(IState<TState, TImpulse> state)
        {
            Debug.Assert(state != null, "State must not be null");

            if (!Enum.IsDefined(typeof(TState), state.Id))
                throw new InvalidOperationException($"State '{state.Id}' not defined in this FSM.");

            states.Add((TState)(ValueType)state.Id, state);
            state.FsmId = Id;
        }

        public int GetNextStateId(int currentStateId, int impulseId, params object[] arguments)
        {
            Dictionary<TImpulse, TState> transition;
            if (!transitions.TryGetValue((TState)(ValueType)currentStateId, out transition))
                return -1;

            TState toState;
            if (transition.TryGetValue((TImpulse)(ValueType)impulseId, out toState))
                return (int)(ValueType)toState;
            else
                return -1;
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

        public void AddOnEnterState(TState enteredState, TImpulse fromImpulse, Action<ICore, int> action)
        {
            Dictionary<TImpulse, Action<ICore, int>> fromImpulses;

            if (!onEnterActions.TryGetValue(enteredState, out fromImpulses))
            {
                fromImpulses = new Dictionary<TImpulse, Action<ICore, int>>();
                onEnterActions.Add(enteredState, fromImpulses);
            }

            fromImpulses[fromImpulse] = action;
        }

        public void AddOnLeaveState(TState leftState, TImpulse toImpulse, Action<ICore, int> action)
        {
            Dictionary<TImpulse, Action<ICore, int>> toImpulses;

            if (!onLeaveActions.TryGetValue(leftState, out toImpulses))
            {
                toImpulses = new Dictionary<TImpulse, Action<ICore, int>>();
                onLeaveActions.Add(leftState, toImpulses);
            }

            toImpulses[toImpulse] = action;
        }

        public void EnterState(Entity entity, int stateId, int withImpulseId)
        {
            states[(TState)(ValueType)stateId].EnterState(entity);

            Dictionary<TImpulse, Action<ICore, int>> fromImpulses;
            if (onEnterActions.TryGetValue(ToState(stateId), out fromImpulses))
                if (fromImpulses.TryGetValue(ToImpulse(withImpulseId), out Action<ICore, int> action))
                    action.Invoke(Core, entity.Id);
        }

        public void LeaveState(Entity entity, int stateId, int withImpulseId)
        {
            states[(TState)(ValueType)stateId].LeaveState(entity);

            Dictionary<TImpulse, Action<ICore, int>> toImpulses;
            if (onLeaveActions.TryGetValue(ToState(stateId), out toImpulses))
                if (toImpulses.TryGetValue(ToImpulse(withImpulseId), out Action<ICore, int> action))
                    action.Invoke(Core, entity.Id);
        }

        public void SetInitialState(Entity entity, int initialStateId)
        {
            Debug.Assert(entity.Contains<FsmComponent>(), $"Entity is missing {nameof(FsmComponent)}");

            var fsmComponent = entity.Get<FsmComponent>();
            Debug.Assert(fsmComponent != null, "Expecting entity containing FsmComponent when setting inital state.");

            var stateData = fsmComponent.States.FirstOrDefault(item => item.FsmId == Id);

            if (stateData != null)
                throw new InvalidOperationException("Initial state already set.");

            stateData = new MachineState() { FsmId = Id, StateId = initialStateId };
            fsmComponent.States.Add(stateData);

            //var state = states[initialState];
            //state.EnterState(entity);
        }

        public string GetCurrentStateName(Entity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();
            var stateData = fsmComponent.States.FirstOrDefault(item => item.FsmId == Id);

            return ToState(stateData.StateId).ToString();
        }

        public string GetStateName(int stateId)
        {
            return ToState(stateId).ToString();
        }

        public int GetStateIdByName(string stateName)
        {
            return (int)Enum.Parse(typeof(TState), stateName);
        }

        public static TState ToState(int stateId)
        {
            return (TState)(ValueType)stateId;
        }

        public static TImpulse ToImpulse(int impulseId)
        {
            return (TImpulse)(ValueType)impulseId;
        }

        public string GetImpulseName(int impulseId)
        {
            return ToImpulse(impulseId).ToString();
        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

        //private void ChangeState(int impulseId, int nextStateId, params object[] arguments)
        //{
        //    currentState.LeaveState();
        //    TryInvokeOnLeaveState(currentState.Id, impulseId);
        //    currentState = states[nextStateId];
        //    TryInvokeOnEnterState(nextStateId, impulseId);
        //    currentState.EnterState();
        //}
    }
}
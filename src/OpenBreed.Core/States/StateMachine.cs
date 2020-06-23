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

        void EnterState(Entity entity, int stateId);

        void LeaveState(Entity entity, int stateId);

        int GetNextStateId(int currentStateId, int impulseId, params object[] arguments);

        void SetInitialState(Entity entity, int initialStateId);

        string GetCurrentStateName(Entity entity);

        string GetStateName(int stateId);

        int GetStateIdByName(string stateName);

        string GetImpulseName(int impulseId);

        #endregion Public Methods
    }

    //public class StateMachine<TState, TImpulse> : IStateMachine where TState : struct, IConvertible where TImpulse : struct, IConvertible
    //{
    //    #region Private Fields

    //    private readonly Dictionary<TState, IState<TState, TImpulse>> states = new Dictionary<TState, IState<TState, TImpulse>>();
    //    private readonly Dictionary<TState, Dictionary<TImpulse, TState>> transitions = new Dictionary<TState, Dictionary<TImpulse, TState>>();
    //    private readonly Dictionary<TState, Dictionary<TImpulse, Action>> onEnterActions = new Dictionary<TState, Dictionary<TImpulse, Action>>();
    //    private readonly Dictionary<TState, Dictionary<TImpulse, Action>> onLeaveActions = new Dictionary<TState, Dictionary<TImpulse, Action>>();
    //    private IState<TState, TImpulse> currentState;

    //    #endregion Private Fields

    //    #region Internal Constructors

    //    internal StateMachine(Entity entity)
    //    {
    //        Entity = entity;
    //    }

    //    #endregion Internal Constructors

    //    #region Public Properties

    //    public string Name => typeof(TState).Name;

    //    public string CurrentStateName => currentState.Id.ToString();

    //    public Entity Entity { get; }

    //    public TState CurrentStateId { get { return currentState.Id; } }

    //    #endregion Public Properties

    //    #region Public Methods

    //    public override string ToString()
    //    {
    //        return $"{typeof(TState)}({CurrentStateId})";
    //    }

    //    public void AddState(IState<TState, TImpulse> state)
    //    {
    //        Debug.Assert(state != null, "State must not be null");

    //        if (!Enum.IsDefined(typeof(TState), state.Id))
    //            throw new InvalidOperationException($"State '{state.Id}' not defined in this FSM.");

    //        states.Add(state.Id, state);
    //    }

    //    /// <summary>
    //    /// Set particular initial state for this state machine
    //    /// </summary>
    //    /// <param name="initialStateId">Initial state id</param>
    //    public void SetInitialState(TState initialStateId)
    //    {
    //        if (currentState != null)
    //            throw new InvalidOperationException($"Initial state already set to '{currentState.Id}'");

    //        currentState = states[initialStateId];
    //    }

    //    public bool Handle(object sender, IMsg cmd)
    //    {
    //        switch (cmd.Type)
    //        {
    //            case EntitySetStateCommand.TYPE:
    //                return HandleStateChangeMsg(sender, (EntitySetStateCommand)cmd);

    //            default:
    //                return false;
    //        }
    //    }

    //    public void Perform(TImpulse impulse, params object[] arguments)
    //    {
    //        Dictionary<TImpulse, TState> transition;
    //        if (transitions.TryGetValue(CurrentStateId, out transition))
    //        {
    //            TState toState;
    //            if (transition.TryGetValue(impulse, out toState))
    //            {
    //                if (!Equals(toState, currentState.Id))
    //                    ChangeState(impulse, toState, arguments);
    //            }
    //            else
    //                Entity.Core.Logging.Warning($"FSM: Transition from state '{CurrentStateId}' using impulse '{impulse}' not defined.");
    //        }
    //        else
    //            Entity.Core.Logging.Warning($"FSM: Transition from state '{CurrentStateId}' not defined.");
    //    }

    //    public void Initialize()
    //    {
    //        if (currentState == null)
    //            throw new InvalidOperationException("Initial state not set");

    //        foreach (var state in states)
    //            state.Value.Initialize(Entity);

    //        currentState.EnterState();
    //    }

    //    public void Deinitialize()
    //    {
    //        currentState.LeaveState();

    //        //foreach (var state in states)
    //        //    state.Value.Deinitialize(Entity);
    //    }

    //    public void AddTransition(TState fromState, TImpulse impulse, TState toState)
    //    {
    //        Dictionary<TImpulse, TState> transition;

    //        if (!transitions.TryGetValue(fromState, out transition))
    //        {
    //            transition = new Dictionary<TImpulse, TState>();
    //            transitions.Add(fromState, transition);
    //        }

    //        transition[impulse] = toState;
    //    }

    //    public void AddOnEnterState(TState enteredState, TImpulse fromImpulse, Action action)
    //    {
    //        Dictionary<TImpulse, Action> fromImpulses;

    //        if (!onEnterActions.TryGetValue(enteredState, out fromImpulses))
    //        {
    //            fromImpulses = new Dictionary<TImpulse, Action>();
    //            onEnterActions.Add(enteredState, fromImpulses);
    //        }

    //        fromImpulses[fromImpulse] = action;
    //    }

    //    public void AddOnLeaveState(TState leftState, TImpulse toImpulse, Action action)
    //    {
    //        Dictionary<TImpulse, Action> toImpulses;

    //        if (!onLeaveActions.TryGetValue(leftState, out toImpulses))
    //        {
    //            toImpulses = new Dictionary<TImpulse, Action>();
    //            onLeaveActions.Add(leftState, toImpulses);
    //        }

    //        toImpulses[toImpulse] = action;
    //    }

    //    #endregion Public Methods

    //    #region Private Methods

    //    private void TryInvokeOnEnterState(TState enteredState, TImpulse fromImpulse)
    //    {
    //        Dictionary<TImpulse, Action> fromImpulses;
    //        if (!onEnterActions.TryGetValue(enteredState, out fromImpulses))
    //            return;

    //        Action action;
    //        if (fromImpulses.TryGetValue(fromImpulse, out action))
    //            action.Invoke();
    //    }

    //    private void TryInvokeOnLeaveState(TState leftState, TImpulse toImpulse)
    //    {
    //        Dictionary<TImpulse, Action> toImpulses;
    //        if (!onLeaveActions.TryGetValue(leftState, out toImpulses))
    //            return;

    //        Action action;
    //        if (toImpulses.TryGetValue(toImpulse, out action))
    //            action.Invoke();
    //    }

    //    private bool HandleStateChangeMsg(object sender, EntitySetStateCommand message)
    //    {
    //        TImpulse impulse;
    //        if (!Enum.TryParse(message.StateId, out impulse))
    //            throw new InvalidOperationException();

    //        Perform(impulse);
    //        return true;
    //    }

    //    private void ChangeState(TImpulse impulse, TState nextState, params object[] arguments)
    //    {
    //        currentState.LeaveState();
    //        TryInvokeOnLeaveState(currentState.Id, impulse);
    //        currentState = states[nextState];
    //        TryInvokeOnEnterState(nextState, impulse);
    //        currentState.EnterState();
    //    }

    //    #endregion Private Methods
    //}

    public class StateMachine<TState, TImpulse> : IStateMachine where TState : Enum where TImpulse : Enum
    {
        #region Private Fields

        private readonly Dictionary<TState, IState<TState, TImpulse>> states = new Dictionary<TState, IState<TState, TImpulse>>();
        private readonly Dictionary<TState, Dictionary<TImpulse, TState>> transitions = new Dictionary<TState, Dictionary<TImpulse, TState>>();
        private readonly Dictionary<TState, Dictionary<TImpulse, Action>> onEnterActions = new Dictionary<TState, Dictionary<TImpulse, Action>>();
        private readonly Dictionary<TState, Dictionary<TImpulse, Action>> onLeaveActions = new Dictionary<TState, Dictionary<TImpulse, Action>>();

        #endregion Private Fields

        #region Internal Constructors

        internal StateMachine(string name)
        {
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

        public void AddOnEnterState(TState enteredState, TImpulse fromImpulse, Action action)
        {
            Dictionary<TImpulse, Action> fromImpulses;

            if (!onEnterActions.TryGetValue(enteredState, out fromImpulses))
            {
                fromImpulses = new Dictionary<TImpulse, Action>();
                onEnterActions.Add(enteredState, fromImpulses);
            }

            fromImpulses[fromImpulse] = action;
        }

        public void AddOnLeaveState(TState leftState, TImpulse toImpulse, Action action)
        {
            Dictionary<TImpulse, Action> toImpulses;

            if (!onLeaveActions.TryGetValue(leftState, out toImpulses))
            {
                toImpulses = new Dictionary<TImpulse, Action>();
                onLeaveActions.Add(leftState, toImpulses);
            }

            toImpulses[toImpulse] = action;
        }

        public void EnterState(Entity entity, int stateId)
        {
            states[(TState)(ValueType)stateId].EnterState(entity);
        }

        public void LeaveState(Entity entity, int stateId)
        {
            states[(TState)(ValueType)stateId].LeaveState(entity);
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

            return ((TState)(ValueType)stateData.StateId).ToString();
        }

        public string GetStateName(int stateId)
        {
            return ((TState)(ValueType)stateId).ToString();
        }

        public int GetStateIdByName(string stateName)
        {
            return (int)Enum.Parse(typeof(TState), stateName);
        }

        public string GetImpulseName(int impulseId)
        {
            return ((TImpulse)(ValueType)impulseId).ToString();
        }

        #endregion Public Methods

        #region Private Methods

        private void TryInvokeOnEnterState(TState enteredState, TImpulse fromImpulse)
        {
            Dictionary<TImpulse, Action> fromImpulses;
            if (!onEnterActions.TryGetValue(enteredState, out fromImpulses))
                return;

            Action action;
            if (fromImpulses.TryGetValue(fromImpulse, out action))
                action.Invoke();
        }

        private void TryInvokeOnLeaveState(TState leftState, TImpulse toImpulse)
        {
            Dictionary<TImpulse, Action> toImpulses;
            if (!onLeaveActions.TryGetValue(leftState, out toImpulses))
                return;

            Action action;
            if (toImpulses.TryGetValue(toImpulse, out action))
                action.Invoke();
        }

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
﻿using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Fsm
{
    public class MachineState
    {
        public const int NO_IMPULSE = -1;

        public MachineState(int fsmId, int initialStateId)
        {
            FsmId = fsmId;
            StateId = initialStateId;
            ImpulseId = NO_IMPULSE;
        }

        #region Public Fields

        public int FsmId { get; }
        public int StateId { get; set; }
        public int ImpulseId { get; set; }

        #endregion Public Fields
    }

    public interface IStateMachine<TState, TImpulse> : IStateMachine where TState : Enum where TImpulse : Enum
    {
        void AddState(IState<TState, TImpulse> state);
        void AddTransition(TState fromState, TImpulse impulse, TState toState);
    }

    public interface IStateMachine
    {
        #region Public Properties

        int Id { get; }
        string Name { get; }

        #endregion Public Properties

        #region Public Methods

        void EnterState(IEntity entity, int stateId, int withImpulseId);

        void LeaveState(IEntity entity, int stateId, int withImpulseId);

        int GetNextStateId(int currentStateId, int impulseId, params object[] arguments);

        void SetInitialState(IEntity entity, int initialStateId);

        string GetCurrentStateName(IEntity entity);

        string GetStateName(int stateId);

        int GetStateIdByName(string stateName);

        string GetImpulseName(int impulseId);

        #endregion Public Methods
    }
}
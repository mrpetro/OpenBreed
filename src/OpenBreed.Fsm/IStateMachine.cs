using OpenBreed.Ecsw.Entities;
using System;

namespace OpenBreed.Fsm
{

    public class MachineState
    {
        #region Public Fields

        public int FsmId;
        public int StateId;

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
}
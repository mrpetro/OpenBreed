using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Fsm
{
    public interface IState
    {
        #region Public Properties

        int Id { get; }
        int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        void EnterState(IEntity entity);

        void LeaveState(IEntity entity);

        #endregion Public Methods
    }

    public interface IState<TState, TImpulse> : IState where TState : Enum where TImpulse : Enum
    {
    }
}
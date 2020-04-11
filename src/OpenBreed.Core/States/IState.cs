using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.States
{
    public interface IState
    {
        #region Public Properties

        IEntity Entity { get; }

        #endregion Public Properties

        #region Public Methods

        void Initialize(IEntity entity);

        void EnterState();

        void LeaveState();

        #endregion Public Methods
    }

    public interface IStateEx
    {
        #region Public Properties

        int Id { get; }

        #endregion Public Properties

        #region Public Methods

        void EnterState(IEntity entity);

        void LeaveState(IEntity entity);

        #endregion Public Methods
    }

    public interface IState<TState, TImpulse> : IState where TState : struct, IConvertible where TImpulse : struct, IConvertible
    {
        #region Public Properties

        TState Id { get; }

        #endregion Public Properties
    }

    public interface IStateEx<TState, TImpulse> : IStateEx where TState : Enum where TImpulse : Enum
    {
        int FsmId { get; set; }
    }
}
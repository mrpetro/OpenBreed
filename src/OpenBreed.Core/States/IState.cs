using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.States
{
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

    public interface IStateEx<TState, TImpulse> : IStateEx where TState : Enum where TImpulse : Enum
    {
        int FsmId { get; set; }
    }
}
﻿using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.States
{
    public interface IState
    {
        #region Public Properties

        int Id { get; }

        #endregion Public Properties

        #region Public Methods

        void EnterState(IEntity entity);

        void LeaveState(IEntity entity);

        #endregion Public Methods
    }

    public interface IState<TState, TImpulse> : IState where TState : Enum where TImpulse : Enum
    {
        int FsmId { get; set; }
    }
}
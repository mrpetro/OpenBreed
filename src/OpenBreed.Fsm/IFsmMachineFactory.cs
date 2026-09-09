using System;

namespace OpenBreed.Fsm
{
    public interface IFsmMachineFactory<TContext>
    {
        #region Public Methods

        IFsmMachine<TContext, TState> GetMachine<TState>() where TState : Enum;

        #endregion Public Methods
    }
}
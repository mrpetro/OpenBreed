using OpenBreed.Fsm.Extensions;
using System;

namespace OpenBreed.Fsm
{
    internal class EntityFsmMachineFactory : IFsmMachineFactory<IEntity>
    {
        #region Public Methods

        public IFsmMachine<IEntity, TState> GetMachine<TState>() where TState : Enum
        {
            return new FsmMachine<IEntity, TState>(GetState<TState>, SetState);
        }

        #endregion Public Methods

        #region Private Methods

        private TState GetState<TState>(IEntity entity) where TState : Enum
        {
            return entity.GetState<TState>();
        }

        private void SetState<TState>(IEntity entity, TState state) where TState : Enum
        {
            entity.SetState(state);
        }

        #endregion Private Methods
    }
}
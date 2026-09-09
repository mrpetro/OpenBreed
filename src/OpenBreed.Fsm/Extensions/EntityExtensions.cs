using System;
using System.Linq;

namespace OpenBreed.Fsm.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static void SetState(this IEntity entity, int fsmId, int impulseId)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            var state = fsmComponent.States.FirstOrDefault(item => item.FsmId == fsmId);

            if (state != null)
                state.ImpulseId = impulseId;
        }

        public static int GetState(this IEntity entity, int fsmId)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            var state = fsmComponent.States.FirstOrDefault(item => item.FsmId == fsmId);

            if (state != null)
                return state.StateId;

            return -1;
        }

        public static void SetState<TState>(this IEntity entity, TState value) where TState : Enum
        {
            var fsmComponent = entity.Get<FsmComponent>();
            fsmComponent.Set(value);
        }

        public static TState GetState<TState>(this IEntity entity) where TState : Enum
        {
            var fsmComponent = entity.Get<FsmComponent>();
            return fsmComponent.Get<TState>();
        }

        #endregion Public Methods
    }
}
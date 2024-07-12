using OpenBreed.Wecs.Entities;
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

        #endregion Public Methods
    }
}
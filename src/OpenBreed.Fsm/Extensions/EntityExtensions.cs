using OpenBreed.Wecs.Entities;
using System.Linq;

namespace OpenBreed.Fsm.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static void SetState(this Entity entity, int fsmId, int impulseId)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            var state = fsmComponent.States.FirstOrDefault(item => item.FsmId == fsmId);

            if (state != null)
                state.ImpulseId = impulseId;
        }

        #endregion Public Methods
    }
}
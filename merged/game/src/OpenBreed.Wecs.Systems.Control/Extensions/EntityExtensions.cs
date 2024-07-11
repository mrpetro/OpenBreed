using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Wecs.Systems.Control.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static void SetControlledEntity(this IEntity entity, int controlledEntityId)
        {
            entity.Get<ControllerComponent>().ControlledEntityId = controlledEntityId;
        }

        public static int GetControlledEntityId(this IEntity entity)
        {
            return entity.Get<ControllerComponent>().ControlledEntityId;
        }

        #endregion Public Methods
    }
}
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Control
{
    [RequireEntityWith(
        typeof(ActionControlComponent))]
    public class ActionControlSystem : UpdatableSystemBase<ActionControlSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Internal Constructors

        internal ActionControlSystem(
            IWorld world,
            IEntityMan entityMan,
            IInputsMan inputsMan,
            IEventsMan eventsMan)
        {
            this.entityMan = entityMan;
            this.inputsMan = inputsMan;
            this.eventsMan = eventsMan;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var actionControlComponent = entity.Get<ActionControlComponent>();
            var controllerComponent = entity.Get<ControllerComponent>();

            if (controllerComponent.ControlledEntityId == -1)
                return;

            var controlledEntity = entityMan.GetById(controllerComponent.ControlledEntityId);

            for (int i = 0; i < actionControlComponent.Mappings.Count; i++)
            {
                var mapping = actionControlComponent.Mappings[i];

                if (inputsMan.IsPressed(mapping.Code))
                    RaiseEntityActionEvent(controlledEntity, mapping.Action);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void RaiseEntityActionEvent(IEntity entity, string actionType)
        {
            eventsMan.Raise(entity, new EntityActionEvent(entity.Id, actionType));
        }

        #endregion Private Methods
    }
}
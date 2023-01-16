using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Control
{
    [RequireEntityWith(
        typeof(ActionControlComponent))]
    public class ActionControlSystem : UpdatableSystemBase<ActionControlSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Internal Constructors

        internal ActionControlSystem(
            IWorld world,
            IEntityMan entityMan,
            IInputsMan inputsMan) :
            base(world)
        {
            this.entityMan = entityMan;
            this.inputsMan = inputsMan;
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

            if (inputsMan.IsPressed(actionControlComponent.Primiary)) Console.WriteLine("Primary action");
            if (inputsMan.IsPressed(actionControlComponent.Secondary)) Console.WriteLine("Secondary action");
        }

        #endregion Protected Methods
    }
}
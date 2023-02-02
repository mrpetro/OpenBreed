using OpenBreed.Common.Interface.Logging;
using OpenBreed.Input.Interface;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Scripting;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Systems
{
    [RequireEntityWith(
        typeof(ActionControlComponent),
        typeof(ControllerComponent))]
    internal class ActorScriptByPlayerControlSystem : UpdatableSystemBase<ActorScriptByPlayerControlSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IInputsMan inputsMan;
        private readonly IScriptMan scriptMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal ActorScriptByPlayerControlSystem(
            IWorld world,
            IEntityMan entityMan,
            IInputsMan inputsMan,
            IScriptMan scriptMan,
            ILogger logger)
        {
            this.entityMan = entityMan;
            this.inputsMan = inputsMan;
            this.scriptMan = scriptMan;
            this.logger = logger;

        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var actionControlComponent = entity.Get<ActionControlComponent>();
            var controlComponent = entity.Get<ControllerComponent>();

            if (controlComponent.ControlledEntityId == -1)
                return;

            var controlledEntity = entityMan.GetById(controlComponent.ControlledEntityId);

            if (inputsMan.IsPressed(actionControlComponent.Primiary))
                controlledEntity.TryInvoke(scriptMan, logger, "onButtonPlayerInput");
        }

        #endregion Protected Methods
    }
}
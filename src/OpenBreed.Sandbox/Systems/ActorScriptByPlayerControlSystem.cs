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
using OpenBreed.Wecs.Systems.Control.Inputs;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Sandbox.Systems
{
    [RequireEntityWith(
        typeof(AttackInputComponent),
        typeof(ControlComponent))]
    internal class ActorScriptByPlayerControlSystem : UpdatableSystemBase<ActorScriptByPlayerControlSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IPlayersMan playersMan;
        private readonly IScriptMan scriptMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal ActorScriptByPlayerControlSystem(
            IWorld world,
            IEntityMan entityMan,
            IPlayersMan playersMan,
            IScriptMan scriptMan,
            ILogger logger) :
            base(world)
        {
            this.entityMan = entityMan;
            this.playersMan = playersMan;
            this.scriptMan = scriptMan;
            this.logger = logger;

        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var inputComponent = entity.Get<AttackInputComponent>();
            var controlComponent = entity.Get<ControlComponent>();

            var player = playersMan.GetById(inputComponent.PlayerId);

            var input = player.Inputs.OfType<ButtonPlayerInput>().FirstOrDefault();

            if (input is null)
                return;

            if (!input.Changed)
                return;

            if (controlComponent.ControlledEntityId == -1)
                return;

            var controlledEntity = entityMan.GetById(controlComponent.ControlledEntityId);

            controlledEntity.TryInvoke(scriptMan, logger, "onButtonPlayerInput");
        }

        #endregion Protected Methods
    }
}
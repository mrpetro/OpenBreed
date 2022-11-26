using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Control.Inputs;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Control
{
    public class AttackControllerSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IPlayersMan playersMan;

        #endregion Private Fields

        #region Internal Constructors

        internal AttackControllerSystem(IPlayersMan playersMan)
        {
            this.playersMan = playersMan;

            RequireEntityWith<AttackInputComponent>();
            RequireEntityWith<AttackControlComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var inputComponent = entity.Get<AttackInputComponent>();
            var control = entity.Get<AttackControlComponent>();

            var player = playersMan.GetById(inputComponent.PlayerId);

            var input = player.Inputs.OfType<ButtonPlayerInput>().FirstOrDefault();

            if (input is null)
                return;

            if (!input.Changed)
                return;

            if (input.Primary != input.OldPrimary)
            {
                if (input.Primary)
                    entity.StartPrimaryAttack();
                else
                    entity.StopPrimaryAttack();
            }
        }

        #endregion Protected Methods
    }
}
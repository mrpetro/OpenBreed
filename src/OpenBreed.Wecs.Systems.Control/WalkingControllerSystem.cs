using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Control.Inputs;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Control
{
    public class WalkingControllerSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IPlayersMan playersMan;

        #endregion Private Fields

        #region Internal Constructors

        internal WalkingControllerSystem(IPlayersMan playersMan)
        {
            this.playersMan = playersMan;

            RequireEntityWith<WalkingInputComponent>();
            RequireEntityWith<WalkingControlComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            var controlComponent = entity.Get<WalkingInputComponent>();
            var walkingControl = entity.Get<WalkingControlComponent>();

            var player = playersMan.GetById(controlComponent.PlayerId);

            var input = player.Inputs.OfType<DigitalJoyPlayerInput>().FirstOrDefault();

            if (input is null)
                return;

            if (!input.Changed)
                return;

            walkingControl.Direction = new Vector2(input.AxisX, input.AxisY);
            entity.RaiseEvent(new ControlDirectionChangedEventArgs(walkingControl.Direction));
        }

        #endregion Protected Methods
    }
}
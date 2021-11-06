using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Control.Inputs;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Control
{
    public class WalkingControllerSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly IPlayersMan playersMan;
        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Internal Constructors

        internal WalkingControllerSystem(IPlayersMan playersMan)
        {
            this.playersMan = playersMan;

            RequireEntityWith<WalkingInputComponent>();
            RequireEntityWith<WalkingControlComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                UpdateEntity(dt, entities[i]);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateEntity(float dt, Entity entity)
        {
            var controlComponent = entity.Get<WalkingInputComponent>();
            var walkingControl = entity.Get<WalkingControlComponent>();

            var player = playersMan.GetById(controlComponent.PlayerId);

            var input = player.Inputs.OfType<DigitalJoyPlayerInput>().FirstOrDefault();

            if (input is null)
                return;

            if (!input.Changed)
                return;

            walkingControl.Direction = new OpenTK.Vector2(input.AxisX, input.AxisY);
            entity.RaiseEvent(new ControlDirectionChangedEventArgs(walkingControl.Direction));
        }

        #endregion Private Methods
    }
}
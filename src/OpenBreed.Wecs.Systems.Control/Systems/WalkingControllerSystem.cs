using OpenBreed.Core;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Control.Systems
{
    public class WalkingControllerSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly IPlayersMan players;
        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Constructors

        public WalkingControllerSystem(ICore core) : base(core)
        {
            players = core.GetManager<IPlayersMan>();

            Require<WalkingInputComponent>();
            Require<WalkingControlComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                UpdateEntity(dt, entities[i]);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

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

            var player = players.GetById(controlComponent.PlayerId);

            var input = player.Inputs.OfType<DigitalJoyPlayerInput>().FirstOrDefault();

            if (input is null)
                return;

            //if (input.Changed)
            //    return;

            var oldDirection = walkingControl.Direction;
            walkingControl.Direction = new OpenTK.Vector2(input.AxisX, input.AxisY);

            if (walkingControl.Direction != oldDirection)
                entity.RaiseEvent(new ControlDirectionChangedEventArgs(walkingControl.Direction));
        }

        #endregion Private Methods
    }
}
using OpenBreed.Core;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Commands;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Control.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Control.Systems
{
    public class AttackControllerSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly IPlayersMan players;
        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Constructors

        public AttackControllerSystem(ICore core) : base(core)
        {
            players = core.GetManager<IPlayersMan>();

            Require<AttackInputComponent>();
            Require<AttackControlComponent>();
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
            var inputComponent = entity.Get<AttackInputComponent>();
            var control = entity.Get<AttackControlComponent>();

            var player = players.GetById(inputComponent.PlayerId);

            var input = player.Inputs.OfType<ButtonPlayerInput>().FirstOrDefault();

            if (input is null)
                return;

            if (!input.Changed)
                return;

            entity.Core.Commands.Post(new AttackControlCommand(entity.Id, entity, input.Primary, input.Secondary));
        }

        #endregion Private Methods
    }
}
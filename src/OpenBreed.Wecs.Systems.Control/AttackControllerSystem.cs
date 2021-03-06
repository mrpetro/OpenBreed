﻿using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Commands;
using OpenBreed.Wecs.Systems.Control.Inputs;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Control
{
    public class AttackControllerSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly IPlayersMan playersMan;
        private readonly ICommandsMan commandsMan;
        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Internal Constructors

        internal AttackControllerSystem(IPlayersMan playersMan, ICommandsMan commandsMan)
        {
            this.playersMan = playersMan;
            this.commandsMan = commandsMan;

            Require<AttackInputComponent>();
            Require<AttackControlComponent>();
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

            var player = playersMan.GetById(inputComponent.PlayerId);

            var input = player.Inputs.OfType<ButtonPlayerInput>().FirstOrDefault();

            if (input is null)
                return;

            if (!input.Changed)
                return;

            commandsMan.Post(new AttackControlCommand(entity.Id, input.Primary, input.Secondary));
        }

        #endregion Private Methods
    }
}
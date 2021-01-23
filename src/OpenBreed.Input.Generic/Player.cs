using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Input.Interface;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Input.Generic
{
    public class Player : IPlayer
    {
        #region Private Fields

        private readonly List<Entity> controlledEntities = new List<Entity>();
        private readonly List<IPlayerInput> inputs = new List<IPlayerInput>();

        #endregion Private Fields

        #region Public Constructors

        public Player(string name, ILogger logger, IInputsMan inputsMan)
        {
            Name = name;
            this.logger = logger;
            this.inputsMan = inputsMan;

            Inputs = new ReadOnlyCollection<IPlayerInput>(inputs);
            ControlledEntities = new ReadOnlyCollection<Entity>(controlledEntities);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<Entity> ControlledEntities { get; }
        private readonly IInputsMan inputsMan;
        private readonly ILogger logger;

        public string Name { get; }
        public ReadOnlyCollection<IPlayerInput> Inputs { get; }

        public void LoseControls()
        {
            controlledEntities.Clear();
        }

        #endregion Public Properties

        #region Public Methods

        public void ResetInputs()
        {
            inputs.ForEach(item => item.Reset(this));
        }

        public void RegisterInput(IPlayerInput input)
        {
            inputs.Add(input);
        }

        public void ApplyInputs()
        {
            inputs.ForEach(item => item.Apply(this));
        }

        public void LoseControl(Entity entity)
        {
            if (!controlledEntities.Remove(entity))
                logger.Warning($"'{entity}' was no under control.");
        }

        public void AssumeControl(Entity entity)
        {
            var controlComponent = entity.TryGet<IControlComponent>();

            if (controlComponent == null)
                throw new InvalidOperationException($"Control on entity '{entity}' not allowed.");

            controlledEntities.Add(entity);
        }

        public void AddKeyBinding(string controlType, string controlAction, Key key)
        {
            inputsMan.AddPlayerKeyBinding(this, controlType, controlAction, key);
        }

        #endregion Public Methods
    }
}
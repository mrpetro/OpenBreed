using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Inputs;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Core
{
    public class Player
    {
        #region Private Fields

        private readonly List<IEntity> controlledEntities = new List<IEntity>();
        private readonly List<IPlayerInput> inputs = new List<IPlayerInput>();

        #endregion Private Fields

        #region Public Constructors

        public Player(string name, ICore core)
        {
            Name = name;
            Core = core;

            Inputs = new ReadOnlyCollection<IPlayerInput>(inputs);
            ControlledEntities = new ReadOnlyCollection<IEntity>(controlledEntities);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<IEntity> ControlledEntities { get; }
        public ICore Core { get; }
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

        public void LoseControl(IEntity entity)
        {
            if (!controlledEntities.Remove(entity))
                Core.Logging.Warning($"'{entity}' was no under control.");
        }

        public void AssumeControl(IEntity entity)
        {
            var controlComponent = entity.TryGetComponent<IControlComponent>();

            if (controlComponent == null)
                throw new InvalidOperationException($"Control on entity '{entity}' not allowed.");

            controlledEntities.Add(entity);
        }

        public void AddKeyBinding(string controlType, string controlAction, Key key)
        {
            Core.Inputs.AddPlayerKeyBinding(this, controlType, controlAction, key);
        }

        #endregion Public Methods
    }
}
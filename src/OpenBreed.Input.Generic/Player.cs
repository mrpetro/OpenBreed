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

        private readonly List<IPlayerInput> inputs = new List<IPlayerInput>();

        #endregion Private Fields

        #region Public Constructors

        public Player(int id, string name, ILogger logger, IInputsMan inputsMan)
        {
            Id = id;
            Name = name;
            this.logger = logger;
            this.inputsMan = inputsMan;

            Inputs = new ReadOnlyCollection<IPlayerInput>(inputs);
        }

        #endregion Public Constructors

        #region Public Properties

        private readonly IInputsMan inputsMan;
        private readonly ILogger logger;

        public int Id { get; }
        public string Name { get; }
        public ReadOnlyCollection<IPlayerInput> Inputs { get; }

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

        public void AddKeyBinding(string controlType, string controlAction, Key key)
        {
            inputsMan.AddPlayerKeyBinding(this, controlType, controlAction, key);
        }

        #endregion Public Methods
    }
}
using OpenBreed.Common.Logging;
using OpenBreed.Input.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Input.Generic
{
    public class PlayersMan : IPlayersMan
    {
        #region Private Fields

        private readonly List<Player> players = new List<Player>();
        private readonly ILogger logger;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Public Constructors

        public PlayersMan(ILogger logger, IInputsMan inputsMan)
        {
            this.logger = logger;
            this.inputsMan = inputsMan;
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public IPlayer AddPlayer(string name)
        {
            if (players.Any(item => item.Name == name))
                throw new InvalidOperationException($"Player with name '{name}' already exists.");

            var newPlayer = new Player(name, logger, inputsMan);
            players.Add(newPlayer);

            return newPlayer;
        }

        public IPlayer GetByName(string name)
        {
            return players.FirstOrDefault(item => item.Name == name);
        }

        public void LooseAllControls()
        {
            players.ForEach(item => item.LoseControls());
        }

        public void ResetInputs()
        {
            players.ForEach(item => item.ResetInputs());
        }

        public void ApplyInputs()
        {
            players.ForEach(item => item.ApplyInputs());
        }

        #endregion Public Methods
    }
}
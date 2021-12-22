using OpenBreed.Common.Logging;
using OpenBreed.Input.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Input.Generic
{
    internal class PlayersMan : IPlayersMan
    {
        #region Private Fields

        private readonly List<Player> players = new List<Player>();
        private readonly ILogger logger;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Internal Constructors

        public PlayersMan(ILogger logger, IInputsMan inputsMan)
        {
            this.logger = logger;
            this.inputsMan = inputsMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IPlayer AddPlayer(string name)
        {
            if (players.Any(item => item.Name == name))
                throw new InvalidOperationException($"Player with name '{name}' already exists.");

            var newPlayer = new Player(players.Count, name, logger, inputsMan);
            players.Add(newPlayer);

            return newPlayer;
        }

        public IPlayer GetByName(string playerName)
        {
            return players.FirstOrDefault(item => item.Name == playerName);
        }

        public IPlayer GetById(int playerId)
        {
            return players.FirstOrDefault(item => item.Id == playerId);
        }

        public void ResetInputs()
        {
            players.ForEach(item => item.ResetInputs());
        }

        #endregion Public Methods
    }
}
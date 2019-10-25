using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core
{
    public class PlayersMan
    {
        #region Private Fields

        private readonly List<Player> players = new List<Player>();

        #endregion Private Fields

        #region Public Constructors

        public PlayersMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public Player AddPlayer(string name)
        {
            if (players.Any(item => item.Name == name))
                throw new InvalidOperationException($"Player with name '{name}' already exists.");

            var newPlayer = new Player(name, Core);
            players.Add(newPlayer);

            return newPlayer;
        }

        public Player GetByName(string name)
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
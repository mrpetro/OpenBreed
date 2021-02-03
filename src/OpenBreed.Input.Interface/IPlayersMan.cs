namespace OpenBreed.Input.Interface
{
    public interface IPlayersMan
    {
        #region Public Methods

        IPlayer AddPlayer(string name);

        IPlayer GetByName(string name);

        IPlayer GetById(int playerId);

        void LooseAllControls();

        void ResetInputs();

        #endregion Public Methods
    }
}
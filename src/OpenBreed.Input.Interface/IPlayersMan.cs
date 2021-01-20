namespace OpenBreed.Input.Interface
{
    public interface IPlayersMan
    {
        #region Public Methods

        IPlayer AddPlayer(string name);

        IPlayer GetByName(string name);

        void LooseAllControls();

        void ResetInputs();

        void ApplyInputs();

        #endregion Public Methods
    }
}
namespace OpenBreed.Core.Managers
{
    public interface IPlayersMan
    {
        #region Public Methods

        Player AddPlayer(string name);

        Player GetByName(string name);

        void LooseAllControls();

        void ResetInputs();

        void ApplyInputs();

        #endregion Public Methods
    }
}
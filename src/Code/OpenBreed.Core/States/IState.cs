namespace OpenBreed.Core.States
{
    public interface IState
    {
        #region Public Properties

        string Id { get; }

        #endregion Public Properties

        #region Public Methods

        void EnterState();

        string Update(float dt);

        void LeaveState();

        #endregion Public Methods
    }
}
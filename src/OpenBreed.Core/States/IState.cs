using OpenBreed.Core.Entities;

namespace OpenBreed.Core.States
{
    public interface IState
    {
        #region Public Properties

        IEntity Entity { get; }

        string Id { get; }

        #endregion Public Properties

        #region Public Methods

        void Initialize(IEntity entity);

        void EnterState();

        void LeaveState();

        string Process(string actionName, object[] arguments);

        #endregion Public Methods
    }
}
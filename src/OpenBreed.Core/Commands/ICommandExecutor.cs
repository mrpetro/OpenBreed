using OpenBreed.Core.Commands;

namespace OpenBreed.Core.Commands
{
    public interface ICommandExecutor
    {
        #region Public Methods

        bool ExecuteCommand(ICommand cmd);

        #endregion Public Methods
    }
}
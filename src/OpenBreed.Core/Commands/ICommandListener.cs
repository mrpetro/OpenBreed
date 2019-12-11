using OpenBreed.Core.Commands;

namespace OpenBreed.Core.Commands
{
    public interface ICommandListener
    {
        #region Public Methods

        bool RecieveCommand(object sender, ICommand cmd);

        #endregion Public Methods
    }
}
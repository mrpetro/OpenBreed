using OpenBreed.Core.Commands;

namespace OpenBreed.Core.Commands
{
    public interface IMsgHandler
    {
        #region Public Methods

        bool Handle(IMsg msg);

        #endregion Public Methods
    }
}
using OpenBreed.Core.Commands;

namespace OpenBreed.Wecs.Commands
{
    public interface IEntityCommand : ICommand
    {
        #region Public Properties

        int EntityId { get; }

        #endregion Public Properties
    }
}
using OpenBreed.Core.Commands;
using System;

namespace OpenBreed.Core.Managers
{
    public interface ICommandHandler
    {
        void Handle(ICommand command);
    }

    public interface ICommandHandler<TCommand> : ICommandHandler where TCommand : ICommand
    {
        #region Public Methods

        bool Handle(TCommand command);

        #endregion Public Methods
    }

    public interface ICommandsMan
    {
        #region Public Methods

        void RegisterHandler<TCommand>(ICommandHandler<TCommand> commandHandler) where TCommand : ICommand;

        void RegisterCommand<TCommand>(ICommandHandler commandHandler) where TCommand : ICommand;

        void Register<TCommand>(Func<TCommand, bool> cmdHandler);

        void ExecuteEnqueued();

        void Post(ICommand msg);

        #endregion Public Methods
    }
}
using OpenBreed.Core.Commands;
using System;

namespace OpenBreed.Core.Managers
{
    public interface ICommandsMan
    {
        #region Public Methods

        void Register<T>(Func<ICore, T, bool> cmdHandler);

        void ExecuteEnqueued(ICore core);

        void Post(ICommand msg);

        #endregion Public Methods
    }
}
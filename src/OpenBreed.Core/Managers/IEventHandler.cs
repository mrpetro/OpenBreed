using System;

namespace OpenBreed.Core.Managers
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<TEvent> : IEventHandler where TEvent : EventArgs
    {
        #region Public Methods

        void Handle(TEvent eventArgs);

        #endregion Public Methods
    }
}
using System;

namespace OpenBreed.Core.Managers
{
    public interface IEventsMan
    {
        #region Public Methods

        void Raise<T>(object sender, T eventArgs) where T : EventArgs;

        void Subscribe<T>(object sender, Action<object, T> callback) where T : EventArgs;

        void Unsubscribe<T>(object sender, Action<object, T> callback) where T : EventArgs;

        #endregion Public Methods
    }
}
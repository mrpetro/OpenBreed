using System;

namespace OpenBreed.Core.Interface.Managers
{
    public delegate void EventCallback<TEventArgs>(TEventArgs e) where TEventArgs : EventArgs;

    public interface IEventsMan
    {
        #region Public Methods

        void Raise<TEventArgs>(object sender, TEventArgs eventArgs) where TEventArgs : EventArgs;

        void Raise<TEventArgs>(TEventArgs eventArgs) where TEventArgs : EventArgs;

        void Subscribe<TEventArgs>(EventCallback<TEventArgs> callback) where TEventArgs : EventArgs;

        void Unsubscribe<TEventArgs>(EventCallback<TEventArgs> callback) where TEventArgs : EventArgs;

        #endregion Public Methods
    }
}
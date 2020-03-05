using System;

namespace OpenBreed.Core.Modules
{
    /// <summary>
    /// Engine core module interface
    /// </summary>
    public interface ICoreModule
    {
        #region Public Properties

        /// <summary>
        /// Reference to core
        /// </summary>
        ICore Core { get; }

        /// <summary>
        /// Subscribe to event of particular type
        /// </summary>
        /// <param name="eventType">Type of event to subscribe for</param>
        /// <param name="callback">Callback action when event will occur</param>
        void Subscribe(string eventType, Action<object, EventArgs> callback);

        #endregion Public Properties
    }
}
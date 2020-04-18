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
        /// 
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="callback">Event callback action</param>
        void Subscribe<T>(Action<object, T> callback) where T : EventArgs;

        #endregion Public Properties
    }
}
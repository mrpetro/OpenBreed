using OpenBreed.Common.Logging;
using System;

namespace OpenBreed.Core.Modules
{
    /// <summary>
    /// Abstract base imlementation for Core module
    /// </summary>
    public abstract class BaseCoreModule : ICoreModule
    {
        #region Protected Constructors

        protected BaseCoreModule(ICore core)
        {
            Core = core;
        }

        #endregion Protected Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void Subscribe<T>(Action<object, T> callback) where T: EventArgs
        {
            Core.Events.Subscribe(this, callback);
        }

        #endregion Public Methods
    }
}
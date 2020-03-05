﻿using System;

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
            Core = core ?? throw new ArgumentNullException(nameof(core));
        }

        #endregion Protected Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void Subscribe(string eventType, Action<object, EventArgs> callback)
        {
            Core.Events.Subscribe(this, eventType, callback);
        }

        #endregion Public Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using System;

namespace OpenBreed.Editor.VM
{
    public class EditorApplication : ApplicationBase, IDisposable
    {
        #region Private Fields

        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors


        public EditorApplication()
        {
            Logger = new DefaultLogger();
            Settings = new SettingsMan(this, Logger);
            Variables = new VariableMan(this, Logger);

            Settings.Restore();
        }

        #endregion Public Constructors

        #region Public Properties

        public ILogger Logger { get; }

        public SettingsMan Settings { get; }

        public VariableMan Variables { get; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Settings.Store();
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}
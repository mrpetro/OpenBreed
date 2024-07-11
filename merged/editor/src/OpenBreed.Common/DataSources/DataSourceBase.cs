using OpenBreed.Common.Data;
using System;
using System.IO;

namespace OpenBreed.Common.DataSources
{
    public abstract class DataSourceBase : IDisposable
    {
        #region Protected Fields

        protected readonly DataSourceProvider provider;

        #endregion Protected Fields

        #region Private Fields

        private Stream _stream;

        #endregion Private Fields

        #region Protected Constructors

        protected DataSourceBase(DataSourceProvider provider, string id)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Id = id;
        }

        #endregion Protected Constructors

        #region Public Properties

        public string Id { get; }

        public Stream Stream
        {
            get
            {
                if (_stream is null)
                {
                    _stream = Open();
                }

                return _stream;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            if (_stream != null)
            {
                Close();
            }
        }

        public virtual Stream Open()
        {
            provider.LockDataSource(this);
            return CreateStream();
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Close()
        {
            provider.ReleaseDataSource(this);
        }

        protected abstract Stream CreateStream();

        #endregion Protected Methods
    }
}
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Common.Assets
{
    public abstract class AssetBase : IDisposable
    {

        #region Protected Fields

        protected readonly AssetsDataProvider _manager;

        #endregion Protected Fields

        #region Private Fields

        private readonly List<FormatParameter> _parameters;
        private IDataFormatType _formatType;
        private Stream _stream;

        #endregion Private Fields

        #region Protected Constructors

        protected AssetBase(AssetsDataProvider manager, string id, IDataFormatType formatType, List<FormatParameter> parameters)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));

            Id = id;
            _formatType = formatType;
            _parameters = parameters;
        }

        #endregion Protected Constructors

        #region Public Properties

        public string Id { get; }

        public Stream Stream
        {
            get
            {
                if (_stream == null)
                    _stream = Open();

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

        public object Load()
        {
            return _formatType.Load(this, _parameters);
        }

        public virtual Stream Open()
        {
            _manager.LockSource(this);
            return CreateStream();
        }

        public void Save(object model)
        {
            _formatType.Save(this, model, _parameters);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Close()
        {
            _manager.ReleaseSource(this);
        }
        protected abstract Stream CreateStream();

        #endregion Protected Methods

    }
}
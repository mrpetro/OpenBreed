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

        private Stream _stream;
        private IDataFormatType _format;

        #endregion Private Fields

        #region Protected Constructors

        protected AssetBase(AssetsDataProvider manager, IDataFormatType format, string name)
        {
            if (manager == null)
                throw new ArgumentNullException("Manager");

            _manager = manager;
            Name = name;
            _format = format;
        }

        #endregion Protected Constructors

        #region Public Properties

        public string Name { get; }

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

        public object Load(IDataFormatType format, List<FormatParameter> parameters)
        {
            return format.Load(this, parameters);
        }

        public virtual Stream Open()
        {
            _manager.LockSource(this);
            return CreateStream();
        }

        public void Save(object model, IDataFormatType format, List<FormatParameter> parameters)
        {
            format.Save(this, model, parameters);
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
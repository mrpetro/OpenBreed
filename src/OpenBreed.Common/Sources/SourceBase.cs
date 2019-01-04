using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Common.Sources
{
    public abstract class SourceBase : IEntry, IDisposable
    {
        #region Protected Fields

        protected readonly DataSourceProvider _repository;

        #endregion Protected Fields

        #region Private Fields

        private Stream _stream;

        #endregion Private Fields

        #region Protected Constructors

        protected SourceBase(DataSourceProvider manager, string name)
        {
            if (manager == null)
                throw new ArgumentNullException("Manager");

            _repository = manager;
            Name = name;
        }

        #endregion Protected Constructors

        #region Public Properties

        public long Id { get; }
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
            _repository.LockSource(this);
            return CreateStream();
        }

        public void Save(object model, IDataFormatType format)
        {
            format.Save(this, model);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Close()
        {
            _repository.ReleaseSource(this);
        }
        protected abstract Stream CreateStream();

        #endregion Protected Methods

    }
}
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Common.Sources
{
    public abstract class SourceBase : EntityBase, IDisposable
    {

        #region Private Fields

        protected readonly SourcesRepository _manager;

        private Stream _stream;

        #endregion Private Fields

        #region Protected Constructors

        protected SourceBase(SourcesRepository manager, string name)
        {
            if (manager == null)
                throw new ArgumentNullException("Manager");

            _manager = manager;
            Name = name;
        }

        #endregion Protected Constructors

        #region Public Properties

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

        public object Load(IDataFormat format, Dictionary<string, object> parameters)
        {
            return format.Load(this, parameters);
        }

        public virtual Stream Open()
        {
            _manager.LockSource(this);
            return CreateStream();
        }

        public void Save(object model, IDataFormat format)
        {
            format.Save(this, model);
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
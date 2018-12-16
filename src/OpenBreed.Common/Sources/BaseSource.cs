﻿using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Common.Sources
{
    public abstract class BaseSource : IDisposable
    {
        #region Private Fields

        private readonly SourceMan _manager;

        private Stream m_Stream;

        #endregion Private Fields

        #region Protected Constructors

        protected BaseSource(SourceMan manager, SourceDef sourceDef)
        {
            if (manager == null)
                throw new ArgumentNullException("Manager");

            if (sourceDef == null)
                throw new ArgumentNullException("SourceDef");

            _manager = manager;
            Name = sourceDef.Name;
        }

        #endregion Protected Constructors

        #region Public Properties

        public string Name { get; private set; }

        public Stream Stream
        {
            get
            {
                if (m_Stream == null)
                    m_Stream = Open();

                return m_Stream;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            if (m_Stream != null)
            {
                Close();
            }
        }

        public object Load(IDataFormat format, Dictionary<string, object> parameters)
        {
            return format.Load(this, parameters);
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

        protected abstract Stream Open();

        #endregion Protected Methods
    }
}
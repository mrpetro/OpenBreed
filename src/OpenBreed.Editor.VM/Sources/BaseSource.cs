using OpenBreed.Common.Database.Sources;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Editor.VM.Sources
{
    public abstract class BaseSource : IDisposable
    {
        #region Private Fields

        private readonly ISourceFormat _format;
        private readonly SourcesHandler _manager;
        private readonly Dictionary<string, object> _parameters;

        private Stream m_Stream;

        #endregion Private Fields

        #region Protected Constructors

        protected BaseSource(SourcesHandler manager, SourceDef sourceDef)
        {
            if (manager == null)
                throw new ArgumentNullException("Manager");

            if (sourceDef == null)
                throw new ArgumentNullException("SourceDef");

            _manager = manager;
            _format = manager.GetFormatMan(sourceDef.Type);
            _parameters = manager.GetParameters(sourceDef.Parameters);
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


        public T GetParameter<T>(string name)
        {
            object found;
            if (_parameters.TryGetValue(name, out found))
                return (T)found;
            else
                return default(T);
        }

        public void Dispose()
        {
            if (m_Stream != null)
            {
                Close();
            }
        }

        public object Load()
        {
            return _format.Load(this);
        }

        public void Save(object model)
        {
            _format.Save(this, model);
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
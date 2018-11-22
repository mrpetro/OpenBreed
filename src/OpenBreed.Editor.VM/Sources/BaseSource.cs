using OpenBreed.Editor.VM.Database.Sources;
using System;
using System.IO;

namespace OpenBreed.Editor.VM.Sources
{
    public abstract class BaseSource : IDisposable
    {
        #region Private Fields

        private readonly ISourceFormat m_Format;
        private readonly SourcesHandler m_Manager;
        private Stream m_Stream;

        #endregion Private Fields

        #region Protected Constructors

        protected BaseSource(SourcesHandler manager, SourceDef sourceDef)
        {
            if (manager == null)
                throw new ArgumentNullException("Manager");

            if (sourceDef == null)
                throw new ArgumentNullException("SourceDef");

            m_Manager = manager;
            m_Format = manager.GetFormatMan(sourceDef.Type);
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

        public object Load()
        {
            return m_Format.Load(this);
        }

        public void Save(object model)
        {
            m_Format.Save(this, model);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Close()
        {
            m_Manager.ReleaseSource(this);
        }

        protected abstract Stream Open();

        #endregion Protected Methods
    }
}
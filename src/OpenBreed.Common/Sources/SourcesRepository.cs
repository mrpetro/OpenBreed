using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Formats;
using EPF;
using OpenBreed.Common.Logging;
using System.ComponentModel;
using System.Globalization;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Tables.Sources;

namespace OpenBreed.Common.Sources
{
    public delegate string ExpandVariablesDelegate(string text);

    public class SourcesRepository : IRepository<SourceBase>
    {
        private readonly DatabaseSourceTableDef _table;
        private XmlDatabase _context;

        #region Private Fields

        private readonly Dictionary<string, SourceBase> _openedSources = new Dictionary<string, SourceBase>();
        private Dictionary<string, EPFArchive> _openedArchives = new Dictionary<string, EPFArchive>();

        #endregion Private Fields

        #region Public Constructors

        public SourcesRepository(XmlDatabase context)
        {
            _context = context;

            _table = _context.GetSourcesTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public static ExpandVariablesDelegate ExpandVariables { get; set; }

        #endregion Public Properties

        #region Public Methods

        internal void LockSource(SourceBase source)
        {
            _openedSources.Add(source.Name, source);
        }

        internal void ReleaseSource(SourceBase source)
        {
            _openedSources.Remove(source.Name);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void CloseAll()
        {
            foreach (var openedArchive in _openedArchives)
                openedArchive.Value.Dispose();

            _openedArchives.Clear();
        }

        internal EPFArchive GetArchive(string archivePath)
        {
            string normalizedPath = OpenBreed.Common.Tools.GetNormalizedPath(archivePath);

            EPFArchive archive = null;
            if (!_openedArchives.TryGetValue(normalizedPath, out archive))
            {
                File.Copy(normalizedPath, normalizedPath + ".bkp", true);
                archive = EPFArchive.ToExtract(File.Open(normalizedPath, FileMode.Open), true);
                _openedArchives.Add(normalizedPath, archive);
            }

            return archive;
        }

        #endregion Internal Methods

        #region Private Methods

        private SourceBase CreateDirectoryFileSource(DirectoryFileSourceDef sourceDef)
        {
            return new DirectoryFileSource(this, sourceDef.DirectoryPath, sourceDef.Name);
        }

        private SourceBase CreateEPFArchiveSource(EPFArchiveFileSourceDef sourceDef)
        {
            return new EPFArchiveFileSource(this, sourceDef.ArchivePath, sourceDef.Name);
        }

        private SourceBase CreateSource(SourceDef sourceDef)
        {
            if (sourceDef is DirectoryFileSourceDef)
                return CreateDirectoryFileSource((DirectoryFileSourceDef)sourceDef);
            else if (sourceDef is EPFArchiveFileSourceDef)
                return CreateEPFArchiveSource((EPFArchiveFileSourceDef)sourceDef);
            else
                throw new NotImplementedException("Unknown sourceDef");
        }

        public SourceBase GetById(long id)
        {
            throw new NotImplementedException();
        }

        public SourceBase GetByName(string name)
        {
            SourceBase source = null;
            if (_openedSources.TryGetValue(name, out source))
                return source;

            var sourceDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (sourceDef == null)
                throw new Exception("No Source definition found with name: " + name);

            source = CreateSource(sourceDef);

            return source;
        }

        public void Add(SourceBase entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(SourceBase entity)
        {
            throw new NotImplementedException();
        }

        public void Update(SourceBase entity)
        {
            throw new NotImplementedException();
        }

        #endregion Private Methods

    }
}

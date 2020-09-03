using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EPF;
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;

namespace OpenBreed.Common.DataSources
{
    public class EPFArchiveFileDataSource : DataSourceBase
    {

        #region Private Fields

        private EPFArchiveEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public EPFArchiveFileDataSource(DataSourceProvider provider, string id, string archivePath, string entryName) :
            base(provider, id)
        {
            ArchivePath = archivePath;
            EntryName = entryName;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ArchivePath { get; }
        public string EntryName { get; }

        #endregion Public Properties

        #region Protected Methods

        protected override void Close()
        {
            if (_entry == null)
                throw new InvalidOperationException($"Entry {Id} not opened.");

            _entry.Dispose();

            base.Close();
        }

        protected override Stream CreateStream()
        {
            InitEntry();

            return _entry.Open();
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitEntry()
        {
            if (_entry != null)
                throw new InvalidOperationException($"Entry {EntryName} already initialized.");

            var archive = provider.GetArchive(provider.ExpandVariables(ArchivePath));
            _entry = archive.FindEntry(EntryName);
        }

        #endregion Private Methods

    }
}

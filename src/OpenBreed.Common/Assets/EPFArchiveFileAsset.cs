using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EPF;
using OpenBreed.Common.Data;

namespace OpenBreed.Common.Assets
{
    public class EPFArchiveFileAsset : AssetBase
    {

        #region Private Fields

        private EPFArchiveEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public EPFArchiveFileAsset(AssetsDataProvider manager, string name, string archivePath, string entryName) :
            base(manager, name)
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
                throw new InvalidOperationException($"Entry {Name} not opened.");

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

            var archive = _repository.GetArchive(AssetsDataProvider.ExpandVariables(ArchivePath));
            _entry = archive.FindEntry(EntryName);
        }

        #endregion Private Methods
    }
}

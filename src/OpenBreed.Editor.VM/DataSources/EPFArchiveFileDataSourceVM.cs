using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Xml.Items.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.DataSources
{
    public class EPFArchiveFileDataSourceVM : DataSourceVM
    {

        #region Private Fields

        private string _archivePath;
        private string _entryName;

        #endregion Private Fields

        #region Public Properties

        public string ArchivePath
        {
            get { return _archivePath; }
            set { SetProperty(ref _archivePath, value); }
        }

        public string EntryName
        {
            get { return _entryName; }
            set { SetProperty(ref _entryName, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IEPFArchiveDataSourceEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IEPFArchiveDataSourceEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IEPFArchiveDataSourceEntry entry)
        {
            ArchivePath = entry.ArchivePath;
            EntryName = entry.EntryName;
        }

        private void ToEntry(IEPFArchiveDataSourceEntry entry)
        {
            entry.ArchivePath = ArchivePath;
            entry.EntryName = EntryName;
        }

        #endregion Private Methods

    }
}

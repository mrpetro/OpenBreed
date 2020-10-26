using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.DataSources
{
    public class EpfArchiveFileDataSourceEditorVM : BaseViewModel, IEntryEditor<IDataSourceEntry>
    {
        #region Private Fields

        private string _archivePath;
        private string _entryName;

        #endregion Private Fields

        #region Internal Constructors

        internal EpfArchiveFileDataSourceEditorVM(ParentEntryEditor<IDataSourceEntry> parent)
        {
            Parent = parent;
        }

        #endregion Internal Constructors

        #region Public Properties

        public ParentEntryEditor<IDataSourceEntry> Parent { get; }

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

        #region Public Methods

        public void UpdateVM(IDataSourceEntry entry)
        {
            var epfEntry = (IEPFArchiveDataSourceEntry)entry;

            ArchivePath = epfEntry.ArchivePath;
            EntryName = epfEntry.EntryName;
        }

        public void UpdateEntry(IDataSourceEntry entry)
        {
            var epfEntry = (IEPFArchiveDataSourceEntry)entry;

            epfEntry.ArchivePath = ArchivePath;
            epfEntry.EntryName = EntryName;
        }

        #endregion Public Methods
    }
}
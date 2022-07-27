using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.DataSources
{
    public class EpfArchiveFileDataSourceEditorVM : BaseViewModel, IEntryEditor<IDbDataSource>, IEntryEditor<IDbEpfArchiveDataSource>
    {
        #region Private Fields

        private string _archivePath;
        private string _entryName;

        #endregion Private Fields

        #region Public Constructors

        public EpfArchiveFileDataSourceEditorVM()
        {
        }

        #endregion Public Constructors

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

        #region Public Methods

        public void UpdateVM(IDbDataSource entry) => UpdateVM((IDbEpfArchiveDataSource)entry);

        public void UpdateEntry(IDbDataSource entry) => UpdateEntry((IDbEpfArchiveDataSource)entry);

        public void UpdateEntry(IDbEpfArchiveDataSource entry)
        {
            entry.ArchivePath = ArchivePath;
            entry.EntryName = EntryName;
        }

        public void UpdateVM(IDbEpfArchiveDataSource entry)
        {
            ArchivePath = entry.ArchivePath;
            EntryName = entry.EntryName;
        }

        #endregion Public Methods
    }
}
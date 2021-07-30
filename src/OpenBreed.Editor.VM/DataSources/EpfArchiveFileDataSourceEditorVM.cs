using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.DataSources
{
    public class EpfArchiveFileDataSourceEditorVM : BaseViewModel, IEntryEditor<IDbDataSource>
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

        public void UpdateVM(IDbDataSource entry)
        {
            var epfEntry = (IDbEpfArchiveDataSource)entry;

            ArchivePath = epfEntry.ArchivePath;
            EntryName = epfEntry.EntryName;
        }

        public void UpdateEntry(IDbDataSource entry)
        {
            var epfEntry = (IDbEpfArchiveDataSource)entry;

            epfEntry.ArchivePath = ArchivePath;
            epfEntry.EntryName = EntryName;
        }

        #endregion Public Methods
    }
}
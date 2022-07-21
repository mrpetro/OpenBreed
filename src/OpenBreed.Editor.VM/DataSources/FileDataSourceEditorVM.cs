using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.DataSources
{
    public class FileDataSourceEditorVM : BaseViewModel, IEntryEditor<IDbDataSource>, IEntryEditor<IDbFileDataSource>
    {
        #region Private Fields

        private string _filePath;

        #endregion Private Fields

        #region Public Constructors

        public FileDataSourceEditorVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void UpdateVM(IDbDataSource entry) => UpdateVM((IDbFileDataSource)entry);

        public void UpdateEntry(IDbDataSource entry) => UpdateEntry((IDbFileDataSource)entry);

        public void UpdateEntry(IDbFileDataSource entry)
        {
            entry.FilePath = FilePath;
        }

        public void UpdateVM(IDbFileDataSource entry)
        {
            FilePath = entry.FilePath;
        }

        #endregion Public Methods
    }
}
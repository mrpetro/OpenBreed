using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.DataSources;

namespace OpenBreed.Editor.VM.DataSources
{
    public class DataSourceEditorVM : ParentEntryEditor<IDataSourceEntry>
    {
        #region Public Constructors

        static DataSourceEditorVM()
        {
            RegisterSubeditor<IFileDataSourceEntry>((parent) => new FileDataSourceEditorVM(parent));
            RegisterSubeditor<IEPFArchiveDataSourceEntry>((parent) => new EpfArchiveFileDataSourceEditorVM(parent));
        }

        public DataSourceEditorVM(EditorApplication application, IRepository repository) : base(application, repository, "Data Source Editor")
        {
        }

        #endregion Public Constructors
    }
}
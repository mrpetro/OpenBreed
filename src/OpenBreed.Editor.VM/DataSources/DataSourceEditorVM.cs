using OpenBreed.Common.Data;
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

        public DataSourceEditorVM(EditorApplication application, DataProvider dataProvider, IUnitOfWork unitOfWork) : base(application, dataProvider, unitOfWork, "Data Source Editor")
        {
        }

        #endregion Public Constructors
    }
}
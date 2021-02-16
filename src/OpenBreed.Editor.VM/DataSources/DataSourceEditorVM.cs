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
            RegisterSubeditor<IFileDataSourceEntry>((parent) => new FileDataSourceEditorVM());
            RegisterSubeditor<IEPFArchiveDataSourceEntry>((parent) => new EpfArchiveFileDataSourceEditorVM());
        }

        public DataSourceEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan, dataProvider, dialogProvider, "Data Source Editor")
        {
        }

        #endregion Public Constructors
    }
}
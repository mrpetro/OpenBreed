using OpenBreed.Common;
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
            RegisterSubeditor<IFileDataSourceEntry, IDataSourceEntry>();
            RegisterSubeditor<IEPFArchiveDataSourceEntry, IDataSourceEntry>();
        }

        public DataSourceEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dataProvider, dialogProvider, "Data Source Editor")
        {
        }

        #endregion Public Constructors
    }
}
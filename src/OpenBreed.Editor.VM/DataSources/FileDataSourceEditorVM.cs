using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.DataSources
{
    public class FileDataSourceEditorVM : DataSourceEditorBaseVM<IDbFileDataSource>
    {
        #region Public Constructors

        public FileDataSourceEditorVM(
            IDbFileDataSource dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string FilePath
        {
            get { return Entry.FilePath; }
            set { SetProperty(Entry, x => x.FilePath, value); }
        }

        public override string EditorName => "File Data Source Editor";

        #endregion Public Properties
    }
}
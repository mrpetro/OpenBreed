using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.DataSources
{
    public class EpfArchiveFileDataSourceEditorVM : DataSourceEditorBaseVM<IDbEpfArchiveDataSource>
    {
        #region Public Constructors

        public EpfArchiveFileDataSourceEditorVM(
            IDbEpfArchiveDataSource dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string ArchivePath
        {
            get { return Entry.ArchivePath; }
            set { SetProperty(Entry, x => x.ArchivePath, value); }
        }

        public string EntryName
        {
            get { return Entry.EntryName; }
            set { SetProperty(Entry, x => x.EntryName, value); }
        }

        public override string EditorName => "EPF Data Source Editor";

        #endregion Public Properties
    }
}
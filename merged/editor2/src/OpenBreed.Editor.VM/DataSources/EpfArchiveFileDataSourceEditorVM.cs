using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.DataSources
{
    public class EpfArchiveFileDataSourceEditorVM : DataSourceEditorBaseVM<IDbEpfArchiveDataSource>
    {
        #region Private Fields

        private string _archivePath;
        private string _entryName;

        #endregion Private Fields

        #region Public Constructors

        public EpfArchiveFileDataSourceEditorVM(
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, workspaceMan, dialogProvider)
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

        public override string EditorName => "EPF Data Source Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(IDbEpfArchiveDataSource entry)
        {
            entry.ArchivePath = ArchivePath;
            entry.EntryName = EntryName;
        }

        protected override void UpdateVM(IDbEpfArchiveDataSource entry)
        {
            ArchivePath = entry.ArchivePath;
            EntryName = entry.EntryName;
        }

        #endregion Protected Methods
    }
}
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.DataSources
{
    public class FileDataSourceEditorVM : DataSourceEditorBaseVM<IDbFileDataSource>
    {
        #region Private Fields

        private string _filePath;

        #endregion Private Fields

        #region Public Constructors

        public FileDataSourceEditorVM(
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IControlFactory controlFactory) : base(workspaceMan, dialogProvider, controlFactory)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        public override string EditorName => "File Data Source Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(IDbFileDataSource entry)
        {
            entry.FilePath = FilePath;
        }

        protected override void UpdateVM(IDbFileDataSource entry)
        {
            FilePath = entry.FilePath;
        }

        #endregion Protected Methods
    }
}
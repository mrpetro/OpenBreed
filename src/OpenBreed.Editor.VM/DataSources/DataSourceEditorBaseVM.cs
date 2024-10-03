using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.EntityTemplates;

namespace OpenBreed.Editor.VM.DataSources
{
    public abstract class DataSourceEditorBaseVM<TDbDataSource> : EntrySpecificEditorVM<IDbDataSource> where TDbDataSource : IDbDataSource
    {
        #region Public Constructors

        public DataSourceEditorBaseVM(
            IDbDataSource dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public new TDbDataSource Entry => (TDbDataSource)base.Entry;

        #endregion Public Properties
    }
}
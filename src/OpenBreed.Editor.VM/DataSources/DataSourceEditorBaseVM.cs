using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.EntityTemplates;

namespace OpenBreed.Editor.VM.DataSources
{
    public abstract class DataSourceEditorBaseVM<TDbDataSource> : EntryEditorBaseVM<IDbDataSource> where TDbDataSource : IDbDataSource
    {
        #region Public Constructors

        public DataSourceEditorBaseVM(
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected abstract void UpdateVM(TDbDataSource entry);

        protected abstract void UpdateEntry(TDbDataSource entry);

        protected override void UpdateVM(IDbDataSource entry)
        {
            base.UpdateVM(entry);

            UpdateVM((TDbDataSource)entry);
        }

        protected override void UpdateEntry(IDbDataSource entry)
        {
            UpdateEntry((TDbDataSource)entry);

            base.UpdateEntry(entry);
        }

        #endregion Protected Methods
    }
}
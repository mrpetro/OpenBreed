using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Interface.Items.Scripts;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public abstract class EntityTemplateEditorBaseVM<TDbEnityTemplate> : EntryEditorBaseVM<IDbEntityTemplate>
    {
        #region Public Constructors

        public EntityTemplateEditorBaseVM(
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IControlFactory controlFactory) : base(workspaceMan, dialogProvider, controlFactory)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected abstract void UpdateVM(TDbEnityTemplate entry);

        protected abstract void UpdateEntry(TDbEnityTemplate entry);

        protected override void UpdateVM(IDbEntityTemplate entry)
        {
            base.UpdateVM(entry);

            UpdateVM((TDbEnityTemplate)entry);
        }

        protected override void UpdateEntry(IDbEntityTemplate entry)
        {
            UpdateEntry((TDbEnityTemplate)entry);

            base.UpdateEntry(entry);
        }

        #endregion Protected Methods
    }
}
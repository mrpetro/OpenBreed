using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Texts;
using System;

namespace OpenBreed.Editor.VM.Texts
{
    public abstract class TextEditorBaseVM<TDbText> : EntryEditorBaseVM<IDbText>
    {
        #region Public Constructors

        public TextEditorBaseVM(
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected abstract void UpdateVM(TDbText entry);

        protected abstract void UpdateEntry(TDbText entry);

        protected override void UpdateVM(IDbText entry)
        {
            base.UpdateVM(entry);

            UpdateVM((TDbText)entry);
        }

        protected override void UpdateEntry(IDbText entry)
        {
            UpdateEntry((TDbText)entry);

            base.UpdateEntry(entry);
        }

        #endregion Protected Methods
    }
}
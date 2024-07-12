using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Texts;
using System;

namespace OpenBreed.Editor.VM.Texts
{
    public abstract class TextEditorBaseVM<TDbText> : EntrySpecificEditorVM<IDbText>
    {
        #region Public Constructors

        public TextEditorBaseVM(
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, workspaceMan, dialogProvider)
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
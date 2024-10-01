using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Texts;
using System;

namespace OpenBreed.Editor.VM.Texts
{
    public abstract class TextEditorBaseVM<TDbText> : EntrySpecificEditorVM<IDbText> where TDbText : IDbText
    {
        #region Public Constructors

        public TextEditorBaseVM(
            TDbText dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public new TDbText Entry => (TDbText)base.Entry;

        #endregion Public Properties

        #region Protected Methods

        protected virtual void UpdateVM() { }

        protected virtual void UpdateEntry() { }

        protected override void UpdateVM(IDbText entry)
        {
            base.UpdateVM(entry);

            UpdateVM();
        }

        protected override void UpdateEntry(IDbText entry)
        {
            UpdateEntry();

            base.UpdateEntry(entry);
        }

        #endregion Protected Methods
    }
}
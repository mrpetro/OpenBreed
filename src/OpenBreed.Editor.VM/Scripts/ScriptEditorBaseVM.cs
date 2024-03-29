using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM.Scripts
{
    public abstract class ScriptEditorBaseVM<TDbScript> : EntryEditorBaseVM<IDbScript>
    {
        #region Public Constructors

        public ScriptEditorBaseVM(
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected abstract void UpdateVM(TDbScript entry);

        protected abstract void UpdateEntry(TDbScript entry);

        protected override void UpdateVM(IDbScript entry)
        {
            base.UpdateVM(entry);

            UpdateVM((TDbScript)entry);
        }

        protected override void UpdateEntry(IDbScript entry)
        {
            UpdateEntry((TDbScript)entry);

            base.UpdateEntry(entry);
        }

        #endregion Protected Methods
    }
}
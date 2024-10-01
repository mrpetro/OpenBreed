using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.Scripts;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM.Scripts
{
    public abstract class ScriptEditorBaseVM<TDbScript> : EntrySpecificEditorVM<IDbScript> where TDbScript : IDbScript
    {
        #region Public Constructors

        public ScriptEditorBaseVM(
            TDbScript dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public new TDbScript Entry => (TDbScript)base.Entry;

        #endregion Public Properties

        #region Protected Methods

        protected virtual void UpdateVM() { }

        protected virtual void UpdateEntry() { }

        protected override void UpdateVM(IDbScript entry)
        {
            base.UpdateVM(entry);

            UpdateVM();
        }

        protected override void UpdateEntry(IDbScript entry)
        {
            UpdateEntry();

            base.UpdateEntry(entry);
        }

        #endregion Protected Methods
    }
}
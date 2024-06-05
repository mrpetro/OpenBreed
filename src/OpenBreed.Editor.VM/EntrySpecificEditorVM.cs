using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM
{
    public abstract class EntrySpecificEditorVM : BaseViewModel
    {
        #region Protected Constructors

        protected EntrySpecificEditorVM()
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        public abstract string EditorName { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract void UpdateVM(IDbEntry source);

        public abstract void UpdateEntry(IDbEntry target);

        #endregion Public Methods
    }

    public abstract class EntrySpecificEditorVM<TDbEntry> : EntrySpecificEditorVM where TDbEntry : IDbEntry
    {
        #region Private Fields

        private readonly IDialogProvider dialogProvider;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Protected Constructors

        protected EntrySpecificEditorVM(
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider)
        {
            WorkspaceMan = workspaceMan;
            this.dialogProvider = dialogProvider;
            this.logger = logger;
        }

        #endregion Protected Constructors

        #region Public Properties

        public TDbEntry EditedEntry { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal IWorkspaceMan WorkspaceMan { get; }

        internal IDialogProvider DialogProvider => dialogProvider;

        #endregion Internal Properties

        #region Public Methods

        public override void UpdateEntry(IDbEntry target)
        {
            UpdateEntry((TDbEntry)target);
        }

        public override void UpdateVM(IDbEntry source)
        {
            try
            {
                UpdateVM((TDbEntry)source);
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to load '{0}' entry. Exception:{2}", source.Id,
                ex);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void UpdateEntry(TDbEntry target)
        {
        }

        protected virtual void UpdateVM(TDbEntry source)
        {
            EditedEntry = source;
        }

        #endregion Protected Methods
    }
}
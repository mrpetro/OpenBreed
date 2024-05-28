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

        #endregion Private Fields

        #region Protected Constructors

        protected EntrySpecificEditorVM(
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IControlFactory controlFactory)
        {
            WorkspaceMan = workspaceMan;
            this.dialogProvider = dialogProvider;
        }

        #endregion Protected Constructors

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
            UpdateVM((TDbEntry)source);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void UpdateEntry(TDbEntry target)
        {
        }

        protected virtual void UpdateVM(TDbEntry source)
        {
        }

        #endregion Protected Methods
    }
}
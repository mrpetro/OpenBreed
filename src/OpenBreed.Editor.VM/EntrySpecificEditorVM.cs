using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace OpenBreed.Editor.VM
{
    public abstract class EntrySpecificEditorVM : BaseViewModel
    {
        #region Private Fields

        private static readonly HashSet<string> propertyNamesIgnoredForChanges = new HashSet<string>();
        private bool changesTrackingEnabled;

        #endregion Private Fields

        #region Protected Constructors

        protected EntrySpecificEditorVM(IDbEntry dbEntry)
        {
            Entry = dbEntry;
        }

        #endregion Protected Constructors

        #region Public Properties

        public abstract string EditorName { get; }
        public Action<string> IdChangedCallback { get; set; }
        public Action DataChangedCallback { get; set; }

        public string Id
        {
            get { return Entry.Id; }
            set { SetProperty(Entry, x => x.Id, value); }
        }

        public string Description
        {
            get { return Entry.Description; }
            set { SetProperty(Entry, x => x.Description, value); }
        }

        #endregion Public Properties

        #region Protected Properties

        protected IDbEntry Entry { get; }

        #endregion Protected Properties

        #region Public Methods

        public abstract void UpdateVM();

        public abstract void UpdateEntry();

        #endregion Public Methods

        #region Protected Methods

        protected static void IgnoreProperty(string propertyName)
        {
            propertyNamesIgnoredForChanges.Add(propertyName);
        }

        protected virtual void DisableChangesTracking()
        {
            changesTrackingEnabled = false;
        }

        protected virtual void EnableChangesTracking()
        {
            changesTrackingEnabled = true;
        }

        protected override void OnPropertyChanged(string name)
        {
            if (changesTrackingEnabled && !IsPropertyIgnored(name))
            {
                DataChangedCallback.Invoke();
            }

            switch (name)
            {
                case nameof(Id):
                    IdChangedCallback?.Invoke(Id);
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool IsPropertyIgnored(string propertyName)
        {
            return propertyNamesIgnoredForChanges.Contains(propertyName);
        }

        #endregion Private Methods
    }

    public abstract class EntrySpecificEditorVM<TDbEntry> : EntrySpecificEditorVM where TDbEntry : IDbEntry
    {
        #region Private Fields

        private readonly IDialogProvider dialogProvider;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Protected Constructors

        protected EntrySpecificEditorVM(
            TDbEntry dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry)
        {
            WorkspaceMan = workspaceMan;
            this.dialogProvider = dialogProvider;
            this.logger = logger;
        }

        #endregion Protected Constructors

        #region Public Properties

        public new TDbEntry Entry => (TDbEntry)base.Entry;

        #endregion Public Properties

        #region Internal Properties

        internal IWorkspaceMan WorkspaceMan { get; }

        internal IDialogProvider DialogProvider => dialogProvider;

        #endregion Internal Properties

        #region Public Methods

        public override void UpdateEntry()
        {
            ProtectedUpdateEntry();
        }

        public override void UpdateVM()
        {
            DisableChangesTracking();

            try
            {
                ProtectedUpdateVM();
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to load '{0}' entry. Exception:{2}", Entry.Id, ex);
            }
            finally
            {
                EnableChangesTracking();
            }
        }

        protected virtual void ProtectedUpdateEntry()
        {
        }

        protected virtual void ProtectedUpdateVM()
        {
        }

        #endregion Public Methods
    }
}
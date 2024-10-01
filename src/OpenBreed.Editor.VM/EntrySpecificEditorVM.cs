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
        private string id;

        private bool changesTrackingEnabled;

        private string description;

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
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        #endregion Public Properties

        #region Protected Properties

        protected IDbEntry Entry { get; private set; }

        #endregion Protected Properties

        #region Public Methods

        public virtual void UpdateVM(IDbEntry source)
        {
            Entry = source;
        }

        public virtual void UpdateEntry(IDbEntry target)
        {
        }

        #endregion Public Methods

        #region Protected Methods

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
            if (changesTrackingEnabled)
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

        protected static void IgnoreProperty(string propertyName)
        {
            propertyNamesIgnoredForChanges.Add(propertyName);
        }

        #endregion Protected Methods
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

        public override void UpdateEntry(IDbEntry target)
        {
            UpdateEntry((TDbEntry)target);

            target.Id = Id;
            target.Description = Description;
        }

        public override void UpdateVM(IDbEntry source)
        {
            DisableChangesTracking();
            base.UpdateVM(source);

            try
            {
                Id = source.Id;
                Description = source.Description;

                UpdateVM((TDbEntry)source);
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to load '{0}' entry. Exception:{2}", source.Id, ex);
            }
            finally
            {
                EnableChangesTracking();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(name);
        }

        protected virtual void UpdateEntry(TDbEntry target)
        {
        }

        protected virtual void UpdateVM(TDbEntry source)
        {
        }

        #endregion Protected Methods
    }
}
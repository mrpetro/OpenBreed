using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Common
{
    public class EntryRefIdEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;
        private readonly Type entryType;
        private readonly Action<string> refIdSelectedAction;
        private string _currentRefId;
        private string _selectedRefId;
        private bool _isChanged;
        private bool _isValid;
        private string _validationMessage;
        private bool _confirmEnabled;
        private bool _undoEnabled;

        #endregion Private Fields

        #region Public Constructors

        public EntryRefIdEditorVM(IWorkspaceMan workspaceMan, Type entryType, Action<string> refIdSelectedAction )
        {
            this.workspaceMan = workspaceMan;
            this.entryType = entryType;
            this.refIdSelectedAction = refIdSelectedAction;
            ReferenceHints = new ObservableCollection<string>();

            var hintsRepository = workspaceMan.GetRepository(entryType);

            foreach (var id in hintsRepository.Entries.Select(item => item.Id))
            {
                ReferenceHints.Add(id);
            };

            ConfirmCommand = new Command(ConfirmRefId);
            UndoCommand = new Command(UndoRefId);
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<string> ReferenceHints { get; }

        public string CurrentRefId
        {
            get { return _currentRefId; }
            set { base.SetProperty(ref _currentRefId, value); }
        }

        public string SelectedRefId
        {
            get { return _selectedRefId; }
            set { base.SetProperty(ref _selectedRefId, value); }
        }

        public bool IsChanged
        {
            get { return _isChanged; }
            set { base.SetProperty(ref _isChanged, value); }
        }

        public bool UndoEnabled
        {
            get { return _undoEnabled; }
            set { base.SetProperty(ref _undoEnabled, value); }
        }

        public bool ConfirmEnabled
        {
            get { return _confirmEnabled; }
            set { base.SetProperty(ref _confirmEnabled, value); }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set { base.SetProperty(ref _isValid, value); }
        }

        public string ValidationMessage
        {
            get { return _validationMessage; }
            set { base.SetProperty(ref _validationMessage, value); }
        }

        public ICommand ConfirmCommand { get; }

        public ICommand UndoCommand { get; }

        #endregion Public Properties

        #region Public Methods

        public void ConfirmRefId()
        {
            CurrentRefId = SelectedRefId;
            refIdSelectedAction?.Invoke(CurrentRefId);

            Refresh();
        }

        public void UndoRefId()
        {
            SelectedRefId = CurrentRefId;
        }

        public bool ValidateRefId(string refId)
        {
            //if (refId is null)
            //{
            //    IsValid = true;
            //    ValidationMessage = null;
            //    return true;
            //}


            if (string.IsNullOrWhiteSpace(refId))
            {
                ValidationMessage = "Reference ID can't be empty.";
                IsValid = false;
                return false;
            }

            if (!ReferenceHints.Contains(refId))
            {
                ValidationMessage = $"Asset '{refId}' doesn't exist.";
                IsValid = false;
                return false;
            }

            ValidationMessage = null;
            IsValid = true;
            return true;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(SelectedRefId):
                    ValidateRefId(SelectedRefId);
                    Refresh();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Refresh()
        {
            IsChanged = CurrentRefId != SelectedRefId;
            UndoEnabled = IsChanged;
            ConfirmEnabled = IsChanged && IsValid;
        }

        #endregion Private Methods
    }
}
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTableNewEntryCreatorVM : BaseViewModel
    {
        #region Private Fields

        private bool _createEnabled;
        private EntryTypeVM _selectedEntryType;
        private string _newId;
        private bool _isVisible;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTableNewEntryCreatorVM()
        {
            EntryTypes = new ObservableCollection<EntryTypeVM>();

            CreateCommand = new Command(() => Create());
            CancelCommand = new Command(() => Cancel());
        }

        #endregion Internal Constructors

        #region Public Properties

        public Action CloseAction { get; set; }

        public Func<string, bool> ValidateNewIdFunc { get; set; }

        public Action CreateAction { get; set; }

        public ICommand CreateCommand { get; }

        public ICommand CancelCommand { get; }

        public bool CreateEnabled
        {
            get { return _createEnabled; }
            set { SetProperty(ref _createEnabled, value); }
        }

        public ObservableCollection<EntryTypeVM> EntryTypes { get; }

        public EntryTypeVM SelectedEntryType
        {
            get { return _selectedEntryType; }
            set { SetProperty(ref _selectedEntryType, value); }
        }

        public string NewId
        {
            get { return _newId; }
            set { SetProperty(ref _newId, value); }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Cancel()
        {
            IsVisible = false;

            CloseAction?.Invoke();
        }

        public void Create()
        {
            CreateAction?.Invoke();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(NewId):
                    CreateEnabled = ValidateNewId();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool ValidateNewId()
        {
            return ValidateNewIdFunc.Invoke(NewId);
        }

        #endregion Private Methods
    }
}
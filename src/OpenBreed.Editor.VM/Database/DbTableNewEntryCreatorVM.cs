using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTableNewEntryCreatorVM : BaseViewModel
    {

        #region Private Fields

        private bool _createEnabled;
        private string _newId;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTableNewEntryCreatorVM()
        {
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Internal Constructors

        #region Public Properties

        public Action CloseAction { get; set; }

        public Action CreateAction { get; set; }

        public bool CreateEnabled
        {
            get { return _createEnabled; }
            set { SetProperty(ref _createEnabled, value); }
        }

        public string NewId
        {
            get { return _newId; }
            set { SetProperty(ref _newId, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Close()
        {
            CloseAction?.Invoke();
        }

        public void Create()
        {
            CreateAction?.Invoke();
        }

        #endregion Public Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(NewId):
                    CreateEnabled = ValidateNewId();
                    break;
                default:
                    break;
            }
        }

        private bool ValidateNewId()
        {
            return !string.IsNullOrWhiteSpace(NewId);
        }

        #endregion Private Methods
    }
}

using CommunityToolkit.Mvvm.Input;
using OpenBreed.Database.Interface.Items.EntityClasses;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.EntityClasses
{
    public class EntityClassEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly EntityClassesTreeEditorVM mainEditor;

        private readonly EntityClassNodeVM editedClass;
        private string id;
        private bool isConfirmIdEnabled;
        private bool isCancelIdEnabled;

        #endregion Private Fields

        #region Public Constructors

        public EntityClassEditorVM(EntityClassesTreeEditorVM mainEditor, EntityClassNodeVM editedClass)
        {
            this.mainEditor = mainEditor ?? throw new ArgumentNullException(nameof(mainEditor));
            this.editedClass = editedClass ?? throw new ArgumentNullException(nameof(editedClass));

            ConfirmIdCommand = new RelayCommand(ConfirmId);
            CancelIdCommand = new RelayCommand(CancelId);

            Id = editedClass.Id;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Description
        {
            get => editedClass.Model.Description;
            set => SetProperty(editedClass.Model, x => x.Description, value);
        }

        public bool IsConfirmIdEnabled
        {
            get => isConfirmIdEnabled;
            private set => SetProperty(ref isConfirmIdEnabled, value);
        }

        public bool IsCancelIdEnabled
        {
            get => isCancelIdEnabled;
            private set => SetProperty(ref isCancelIdEnabled, value);
        }

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public ICommand ConfirmIdCommand { get; }
        public ICommand CancelIdCommand { get; }

        #endregion Public Properties

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Id):

                    IsConfirmIdEnabled = ValidateId(Id);
                    IsCancelIdEnabled = Id != editedClass.Id;
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool ValidateId(string newId)
        {
            if (string.IsNullOrEmpty(newId))
            {
                return false;
            }

            return mainEditor.IsUnique(newId);
        }

        private void ConfirmId()
        {
            editedClass.Id = Id;
            mainEditor.UpdateClass(editedClass.Model);
            IsConfirmIdEnabled = false;
            IsCancelIdEnabled = false;
        }

        private void CancelId()
        {
            Id = editedClass.Id;
        }

        #endregion Private Methods
    }
}
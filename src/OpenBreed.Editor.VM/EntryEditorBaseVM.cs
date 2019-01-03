using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public abstract class EntryEditorBaseVM<M,VM> : EntryEditorVM where VM : BaseViewModel, new()
    {
        #region Private Fields

        private VM _editable;
        private M _edited;

        #endregion Private Fields

        #region Public Constructors

        public EntryEditorBaseVM(EditorVM root) : base(root)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public VM Editable
        {
            get { return _editable; }
            set { SetProperty(ref _editable, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void EditModel(M model)
        {
            //Unsubscribe to previous edited item changes
            if (Editable != null)
                Editable.PropertyChanged -= Editable_PropertyChanged;

            _edited = model;

            var vm = new VM();
            UpdateVM(model, vm);
            Editable = vm;
            Editable.PropertyChanged += Editable_PropertyChanged;
        }

        #endregion Public Methods

        #region Internal Methods

        public override void OpenNextEntry()
        {
            throw new NotImplementedException();
        }

        public override void OpenPreviousEntry()
        {
            throw new NotImplementedException();
        }

        public override void OpenEntry(string name)
        {
            var model = GetModel(name);
            EditModel(model);
            EditableName = name;
        }

        #endregion Internal Methods

        #region Protected Methods

        protected abstract M GetModel(string name);
        protected abstract void UpdateModel(VM source, M target);
        protected abstract void UpdateVM(M source, VM target);

        #endregion Protected Methods

        #region Private Methods

        private void Editable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion Private Methods

    }
}

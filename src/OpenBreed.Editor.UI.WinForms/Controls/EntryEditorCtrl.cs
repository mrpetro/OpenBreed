using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.UI.WinForms.Controls.Sounds;

namespace OpenBreed.Editor.UI.WinForms.Controls
{
    public partial class EntryEditorCtrl : UserControl
    {
        #region Private Fields

        private EntryEditorInnerCtrl _innerCtrl;

        private EntryEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public EntryEditorCtrl()
        {
            InitializeComponent();
        }

        public EntryEditorInnerCtrl InnerCtrl
        {
            get { return _innerCtrl; }
            set
            {
                //Remove previous control if it was already added
                if (_innerCtrl != null)
                    Split.Panel2.Controls.Remove(_innerCtrl);

                _innerCtrl = value;

                if (_innerCtrl != null)
                {
                    _innerCtrl.Dock = DockStyle.Fill;
                    Split.Panel2.Controls.Add(_innerCtrl);
                }
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(EntryEditorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            InnerCtrl.Initialize(_vm);

            btnNext.DataBindings.Add(nameof(btnNext.Enabled), _vm, nameof(_vm.NextAvailable), false, DataSourceUpdateMode.OnPropertyChanged);
            btnPrevious.DataBindings.Add(nameof(btnPrevious.Enabled), _vm, nameof(_vm.PreviousAvailable), false, DataSourceUpdateMode.OnPropertyChanged);
            btnCommit.DataBindings.Add(nameof(btnCommit.Enabled), _vm, nameof(_vm.CommitEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            btnRevert.DataBindings.Add(nameof(btnRevert.Enabled), _vm, nameof(_vm.RevertEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            btnCommit.Click += (s, a) => _vm.Commit();
            btnRevert.Click += (s, a) => _vm.Revert();
            btnNext.Click += (s, a) => _vm.EditNextEntry();
            btnPrevious.Click += (s, a) => _vm.EditPreviousEntry();

            _vm.PropertyChanged += _vm_PropertyChanged;

            OnEditableChanged(_vm.Editable);

            OnEditedChanged();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnEditedChanged()
        {
            tbxId.DataBindings.Clear();
            tbxDescription.DataBindings.Clear();
            if (_vm.Id == null)
            {
                tbxId.Text = null;
                tbxDescription.Text = null;
            }
            else
            {
                tbxId.DataBindings.Add(nameof(tbxId.Text), _vm, nameof(_vm.Id), false, DataSourceUpdateMode.OnPropertyChanged);
                tbxDescription.DataBindings.Add(nameof(tbxDescription.Text), _vm, nameof(_vm.Description), false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void OnEditableChanged(EditableEntryVM entry)
        {
            tbxId.DataBindings.Clear();
            tbxDescription.DataBindings.Clear();
            if (entry == null)
            {
                tbxId.Text = null;
                tbxDescription.Text = null;
            }
            else
            {
                tbxId.DataBindings.Add(nameof(tbxId.Text), entry, nameof(entry.Id), false, DataSourceUpdateMode.OnPropertyChanged);
                tbxDescription.DataBindings.Add(nameof(tbxDescription.Text), entry, nameof(entry.Description), false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Editable):
                    OnEditableChanged(_vm.Editable);
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}

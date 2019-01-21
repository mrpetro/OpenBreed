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

            tbxName.DataBindings.Add(nameof(tbxName.Text), _vm, nameof(_vm.EditableName), false, DataSourceUpdateMode.OnPropertyChanged);
            btnNext.DataBindings.Add(nameof(btnNext.Enabled), _vm, nameof(_vm.NextAvailable), false, DataSourceUpdateMode.OnPropertyChanged);
            btnPrevious.DataBindings.Add(nameof(btnPrevious.Enabled), _vm, nameof(_vm.PreviousAvailable), false, DataSourceUpdateMode.OnPropertyChanged);

            btnStore.Click += (s, a) => _vm.Store();
            btnNext.Click += (s, a) => _vm.EditNextEntry();
            btnPrevious.Click += (s, a) => _vm.EditPreviousEntry();

            _vm.PropertyChanged += _vm_PropertyChanged;
            InnerCtrl.Initialize(_vm);
            //UpdateViewState();
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //case nameof(_vm.Editable)
                default:
                    break;
            }
        }

        #endregion Private Methods

        //private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case nameof(_vm.Editable):
        //            UpdateViewState();
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}

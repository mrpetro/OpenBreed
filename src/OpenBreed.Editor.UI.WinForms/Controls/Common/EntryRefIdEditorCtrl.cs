using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.UI.WinForms.Forms.Common;

namespace OpenBreed.Editor.UI.WinForms.Controls.Common
{
    public partial class EntryRefIdEditorCtrl : UserControl
    {
        #region Private Fields

        private EntryRefIdEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public EntryRefIdEditorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(EntryRefIdEditorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(EntryRefIdEditorVM));

            tbxEntryId.DataBindings.Add(nameof(tbxEntryId.Text), _vm, nameof(_vm.RefId), false, DataSourceUpdateMode.OnPropertyChanged);
            btnEntryIdSelect.Click += (s,a) => _vm.SelectEntryId();
            _vm.OpenRefIdSelectorAction = OnOpenRefIdSelector;
        }

        private void OnOpenRefIdSelector(EntryRefIdSelectorVM vm)
        {
            using (var form = new RefIdSelectorForm())
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Initialize(vm);
                if (form.ShowDialog() == DialogResult.OK)
                    vm.Accept();
            }
        }

        #endregion Public Methods
    }
}

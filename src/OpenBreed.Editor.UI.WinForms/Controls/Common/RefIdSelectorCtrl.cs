using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Common;

namespace OpenBreed.Editor.UI.WinForms.Controls.Common
{
    public partial class RefIdSelectorCtrl : UserControl
    {
        private RefSelectorVM _vm;

        public RefIdSelectorCtrl()
        {
            InitializeComponent();
        }

        public void Initialize(RefSelectorVM vm)
        {
            _vm = vm;

            cbxEntryItems.DataSource = _vm.Items;

            cbxEntryItems.DataBindings.Add(nameof(cbxEntryItems.SelectedItem),
                                           _vm, 
                                           nameof(_vm.CurrentEntryId), 
                                           false, 
                                           DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}

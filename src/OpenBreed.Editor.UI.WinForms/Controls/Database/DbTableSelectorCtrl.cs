using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Database;

namespace OpenBreed.Editor.UI.WinForms.Controls.Database
{
    public partial class DbTableSelectorCtrl : UserControl
    {
        private DbTableSelectorVM vm;

        public DbTableSelectorCtrl()
        {
            InitializeComponent();

            cbxTables.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        internal void Initialize(DbTableSelectorVM vm)
        {
            this.vm = vm;

            BindControls();
        }

        private void BindControls()
        {
            cbxTables.DataBindings.Clear();
            cbxTables.DataSource = vm.TableNames;
            cbxTables.DisplayMember = "Name";
            cbxTables.DataBindings.Add(nameof(cbxTables.SelectedItem), vm, nameof(vm.CurrentTableName), false, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}

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
using OpenBreed.Editor.VM.Database.Items;
using OpenBreed.Editor.VM.Database.Tables;

namespace OpenBreed.Editor.UI.WinForms.Controls.Database
{
    public partial class DbTableEditorCtrl : UserControl
    {
        #region Private Fields

        private DbTableEditorVM _vm;

        #endregion Private Fields

        public DbTableEditorCtrl()
        {
            InitializeComponent();

            DGV.AutoGenerateColumns = true;
            DGV.AutoSize = true;
            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DGV.AllowUserToResizeRows = false;
            DGV.RowHeadersVisible = false;

            // Initialize and add a text box column.
            var buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Data";
            buttonColumn.UseColumnTextForButtonValue = true;
            buttonColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            buttonColumn.Width = 60;
            buttonColumn.Name = "Data";
            buttonColumn.Text = "Open";
            DGV.Columns.Add(buttonColumn);


            DGV.CellContentClick += DGV_CellContentClick;
        }

        public void Initialize(DbTableEditorVM vm)
        {
            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            OnEditableChanged(_vm.Editable);
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

        private void OnEditableChanged(DbTableVM dbTable)
        {
            if (dbTable == null)
                DGV.DataSource = null;
            else
                DGV.DataSource = dbTable.Entries;
        }

        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var item = senderGrid.Rows[e.RowIndex].DataBoundItem as DbEntryVM ?? throw new InvalidOperationException();

                _vm.EditEntity(item.Name);
            }
        }
    }
}

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

namespace OpenBreed.Editor.UI.WinForms.Controls.Database
{
    public partial class DatabaseTableViewerCtrl : UserControl
    {
        #region Private Fields

        private DatabaseTableViewerVM _vm;

        #endregion Private Fields

        public DatabaseTableViewerCtrl()
        {
            InitializeComponent();

            DGV.AutoGenerateColumns = true;
            DGV.AutoSize = true;
            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DGV.RowHeadersVisible = false;

            DGV.CellContentClick += DGV_CellContentClick;
        }

        public void Initialize(DatabaseTableViewerVM vm)
        {
            _vm = vm;

            DGV.DataSource = vm.Items;
            // Initialize and add a text box column.
            var buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Data";
            buttonColumn.UseColumnTextForButtonValue = true;
            buttonColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            buttonColumn.Width = 60;
            buttonColumn.Name = "Data";
            buttonColumn.Text = "Open";
            DGV.Columns.Add(buttonColumn);
        }

        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var item = senderGrid.Rows[e.RowIndex].DataBoundItem as DatabaseItemVM ?? throw new InvalidOperationException();

                item.Open();
            }
        }
    }
}

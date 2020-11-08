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
using OpenBreed.Editor.VM.Database.Entries;

namespace OpenBreed.Editor.UI.WinForms.Controls.Database
{
    public partial class DbTableEditorCtrl : UserControl
    {

        #region Private Fields

        private DbTableNewEntryCreatorCtrl _newEntryCreatorCtrl;

        private DbTableEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public DbTableEditorCtrl()
        {
            InitializeComponent();

            SetupDataGridView();

            DGV.MouseClick += DGV_MouseClick;
            DGV.CellMouseDoubleClick += DGV_CellMouseDoubleClick;
        }

        private void DGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var item = DGV.Rows[e.RowIndex].DataBoundItem as DbEntryVM ?? throw new InvalidOperationException();
                _vm.EditEntry(item.Id);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(DbTableEditorVM vm)
        {
            _vm = vm;

            _vm.OpenNewEntryCreatorAction = OnOpenNewEntryCreator;

            DGV.DataSource = _vm.Entries;
        }

        #endregion Public Methods

        #region Private Methods


        private void AddNewEntryCtrl(DbTableNewEntryCreatorVM vm)
        {
            _newEntryCreatorCtrl = new DbTableNewEntryCreatorCtrl();
            _newEntryCreatorCtrl.Dock = DockStyle.Bottom;
            this.Controls.Add(_newEntryCreatorCtrl);
            _newEntryCreatorCtrl.Initialize(vm);
        }

        private void DGV_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var menu = new ContextMenu();
                menu.MenuItems.Add(new MenuItem("New", (s,a) => _vm.OpenNewEntryCreator()));

                int rowUnderMouse = DGV.HitTest(e.X, e.Y).RowIndex;

                if (rowUnderMouse >= 0)
                {
                    var item = DGV.Rows[rowUnderMouse].DataBoundItem as DbEntryVM ?? throw new InvalidOperationException();

                    if (DGV.SelectedRows.Count < 2)
                    {
                        DGV.CurrentCell = DGV.Rows[rowUnderMouse].Cells[0];
                        if (DGV.SelectedRows.Count == 0)
                            DGV.CurrentRow.Selected = true;

                        menu.MenuItems.Add(new MenuItem("Edit", (s, a) => _vm.EditEntry(item.Id)));
                    }

                    menu.MenuItems.Add(new MenuItem("Clone", (s, a) => MessageBox.Show("Not implemented yet.")));
                    menu.MenuItems.Add(new MenuItem("Delete", (s, a) => MessageBox.Show("Not implemented yet.")));
                }

                menu.Show(DGV, new Point(e.X, e.Y));
            }
        }

        private void OnCloseNewEntryCreator()
        {
            if (_newEntryCreatorCtrl != null)
                RemoveNewEntryCtrl();
        }

        private void OnOpenNewEntryCreator(DbTableNewEntryCreatorVM vm)
        {
            if (_newEntryCreatorCtrl != null)
                RemoveNewEntryCtrl();

            vm.CloseAction = OnCloseNewEntryCreator;

            AddNewEntryCtrl(vm);
        }

        private void RemoveNewEntryCtrl()
        {
            if (_newEntryCreatorCtrl == null)
                throw new InvalidOperationException();

            this.Controls.Remove(_newEntryCreatorCtrl);
            _newEntryCreatorCtrl = null;
        }

        private void SetupDataGridView()
        {
            DGV.DataSource = null;

            DGV.Columns.Clear();
            DGV.AutoGenerateColumns = false;
            DGV.AutoSize = true;
            DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DGV.AllowUserToResizeRows = false;
            DGV.AllowUserToAddRows = false;
            DGV.RowHeadersVisible = false;

            var idColumn = new DataGridViewTextBoxColumn();
            idColumn.HeaderText = "Id";
            idColumn.Name = "Id";
            idColumn.DataPropertyName = "Id";
            idColumn.MinimumWidth = 40;
            idColumn.Width = 40;
            idColumn.ReadOnly = true;
            idColumn.Resizable = DataGridViewTriState.False;
            DGV.Columns.Add(idColumn);

            var descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.HeaderText = "Description";
            descriptionColumn.Name = "Description";
            descriptionColumn.DataPropertyName = "Description";
            descriptionColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            descriptionColumn.ReadOnly = false;
            descriptionColumn.Resizable = DataGridViewTriState.False;
            DGV.Columns.Add(descriptionColumn);
        }

        #endregion Private Methods
    }
}

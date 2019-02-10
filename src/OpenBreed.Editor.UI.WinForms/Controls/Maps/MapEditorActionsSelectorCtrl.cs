using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Maps;

namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    public partial class MapEditorActionsSelectorCtrl : UserControl
    {
        #region Private Fields

        private MapEditorActionsSelectorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsSelectorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapEditorActionsSelectorVM vm)
        {
            _vm = vm;

            DGV.CurrentCellDirtyStateChanged += DGV_CurrentCellDirtyStateChanged;

            SetupDataGridView(_vm);
        }

        #endregion Public Methods

        #region Private Methods

        void DGV_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DGV.CurrentCell is DataGridViewCheckBoxCell && DGV.IsCurrentCellDirty)
                DGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void SetupDataGridView(MapEditorActionsSelectorVM vm)
        {
            DGV.DataSource = null;

            DGV.Columns.Clear();
            DGV.RowHeadersVisible = false;
            DGV.AllowUserToAddRows = false;
            DGV.AllowUserToDeleteRows = false;
            DGV.AllowUserToResizeRows = false;
            DGV.MultiSelect = false;
            DGV.AutoGenerateColumns = false;
            DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGV.RowTemplate.Height = 32;

            DataGridViewCheckBoxColumn visibilityColumn = new DataGridViewCheckBoxColumn();
            visibilityColumn.HeaderText = "Visibility";
            visibilityColumn.Name = "Visibility";
            visibilityColumn.DataPropertyName = "Visibility";
            visibilityColumn.MinimumWidth = 50;
            visibilityColumn.Width = 50;
            visibilityColumn.Resizable = DataGridViewTriState.False;
            DGV.Columns.Add(visibilityColumn);

            DataGridViewImageColumn presentationColumn = new DataGridViewImageColumn();
            presentationColumn.HeaderText = "Icon";
            presentationColumn.Name = "Icon";
            presentationColumn.DataPropertyName = "Icon";
            presentationColumn.ImageLayout = DataGridViewImageCellLayout.Normal;
            presentationColumn.DefaultCellStyle.BackColor = Color.Gray;
            presentationColumn.ReadOnly = true;
            presentationColumn.MinimumWidth = 50;
            presentationColumn.Width = 50;
            presentationColumn.Resizable = DataGridViewTriState.False;
            DGV.Columns.Add(presentationColumn);

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.HeaderText = "Id";
            idColumn.Name = "Id";
            idColumn.DataPropertyName = "Id";
            idColumn.MinimumWidth = 40;
            idColumn.Width = 40;
            idColumn.ReadOnly = true;
            idColumn.Resizable = DataGridViewTriState.False;
            DGV.Columns.Add(idColumn);

            DataGridViewTextBoxColumn descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.HeaderText = "Description";
            descriptionColumn.Name = "Description";
            descriptionColumn.DataPropertyName = "Description";
            descriptionColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            descriptionColumn.ReadOnly = true;
            descriptionColumn.Resizable = DataGridViewTriState.False;
            DGV.Columns.Add(descriptionColumn);

            DGV.DataSource = vm.Items;
        }

        #endregion Private Methods
    }
}

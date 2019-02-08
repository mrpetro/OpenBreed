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
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Controls.Actions
{
    public partial class ActionSetEditorCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private ActionSetEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public ActionSetEditorCtrl()
        {
            InitializeComponent();

            DGV.CurrentCellDirtyStateChanged += DGV_CurrentCellDirtyStateChanged;
            DGV.CellContentClick += DGV_CellContentClick;
        }

        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = DGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

            var action = cell.OwningRow.DataBoundItem as ActionVM;

            if (cell.OwningColumn.DataPropertyName == "Icon")
            {
                using (var colorDialog = new ColorDialog())
                {
                    colorDialog.Color = action.Color;

                    var result = colorDialog.ShowDialog();
                    if (result == DialogResult.OK)
                        action.Color = colorDialog.Color;
                } 
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as ActionSetEditorVM ?? throw new InvalidOperationException(nameof(vm));
            _vm.PropertyChanged += _vm_PropertyChanged;

            Update(_vm.Editable);
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Editable):
                    Update(_vm.Editable);
                    break;
                default:
                    break;
            }
        }

        void DGV_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DGV.CurrentCell is DataGridViewCheckBoxCell && DGV.IsCurrentCellDirty)
                DGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void SetNoPropertySetState()
        {
            DGV.DataSource = null;
            DGV.Columns.Clear();
        }

        private void SetPropertySetState(ActionSetVM propertySet)
        {
            SetupDataGridView();
        }

        private void SetupDataGridView()
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

            //DataGridViewTextBoxColumn binaryColumn = new DataGridViewTextBoxColumn();
            //binaryColumn.HeaderText = "Binary";
            //binaryColumn.Name = m_Model.Data.Columns[3].ColumnName;
            //binaryColumn.DataPropertyName = m_Model.Data.Columns[3].ColumnName;
            //binaryColumn.MinimumWidth = 80;
            //binaryColumn.Width = 80;
            //binaryColumn.ReadOnly = true;
            //DataGridView.Columns.Add(binaryColumn);

            DataGridViewTextBoxColumn descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.HeaderText = "Description";
            descriptionColumn.Name = "Description";
            descriptionColumn.DataPropertyName = "Description";
            descriptionColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            descriptionColumn.ReadOnly = false;
            descriptionColumn.Resizable = DataGridViewTriState.False;
            DGV.Columns.Add(descriptionColumn);

            DGV.DataSource = _vm.Editable.Items;
            //DataGridView.DataSource = m_Model.Data;
        }

        void Update(ActionSetVM propertySet)
        {
            if (propertySet == null)
                SetNoPropertySetState();
            else
                SetPropertySetState(propertySet);
        }

        #endregion Private Methods
    }
}

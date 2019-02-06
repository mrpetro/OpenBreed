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
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as ActionSetEditorVM ?? throw new InvalidOperationException(nameof(vm));
            _vm.PropertyChanged += _vm_PropertyChanged;

            DGV.CurrentCellDirtyStateChanged += new EventHandler(DataGridView_CurrentCellDirtyStateChanged);
            DGV.SelectionChanged += new EventHandler(DataGridView_SelectionChanged);

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

        void DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DGV.IsCurrentCellDirty)
            {
                DGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRows = DGV.SelectedRows;
            if (selectedRows.Count > 0)
            {
                var d = DGV.Columns;

                int propertyId = (int)selectedRows[0].Cells["Id"].Value;
                //_vm.Selector.SetSelection(propertyId);
            }
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
            //visibilityColumn.Name = m_Model.Data.Columns[0].ColumnName;
            //visibilityColumn.DataPropertyName = m_Model.Data.Columns[0].ColumnName;
            visibilityColumn.MinimumWidth = 50;
            visibilityColumn.Width = 50;
            DGV.Columns.Add(visibilityColumn);

            DataGridViewImageColumn presentationColumn = new DataGridViewImageColumn();
            presentationColumn.HeaderText = "Icon";
            presentationColumn.Name = "Icon";
            presentationColumn.DataPropertyName = "Icon";
            //presentationColumn.Name = m_Model.Data.Columns[1].ColumnName;
            //presentationColumn.DataPropertyName = m_Model.Data.Columns[1].ColumnName;
            presentationColumn.ImageLayout = DataGridViewImageCellLayout.Normal;
            presentationColumn.DefaultCellStyle.BackColor = Color.Gray;
            presentationColumn.ReadOnly = true;
            presentationColumn.MinimumWidth = 50;
            presentationColumn.Width = 50;
            DGV.Columns.Add(presentationColumn);

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.HeaderText = "Id";
            idColumn.Name = "Id";
            idColumn.DataPropertyName = "Id";
            //idColumn.Name = m_Model.Data.Columns[2].ColumnName;
            //idColumn.DataPropertyName = m_Model.Data.Columns[2].ColumnName;
            idColumn.MinimumWidth = 40;
            idColumn.Width = 40;
            idColumn.ReadOnly = true;
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
            //descriptionColumn.Name = m_Model.Data.Columns[4].ColumnName;
            //descriptionColumn.DataPropertyName = m_Model.Data.Columns[4].ColumnName;
            descriptionColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            descriptionColumn.ReadOnly = true;
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

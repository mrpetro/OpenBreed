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
    public partial class MapEditorActionsToolCtrl : UserControl
    {
        #region Private Fields

        private MapEditorActionsToolVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsToolCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapEditorActionsToolVM vm)
        {
            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            DataGridView.CurrentCellDirtyStateChanged += DataGridView_CurrentCellDirtyStateChanged;
            DataGridView.SelectionChanged += DataGridView_SelectionChanged;

            Update(_vm.CurrentItem);
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.CurrentItem):
                    Update(_vm.CurrentItem);
                    break;
                default:
                    break;
            }
        }

        void DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DataGridView.IsCurrentCellDirty)
            {
                DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRows = DataGridView.SelectedRows;
            if (selectedRows.Count > 0)
            {
                var d = DataGridView.Columns;

                int propertyId = (int)selectedRows[0].Cells["Id"].Value;
                //_vm.Selector.SetSelection(propertyId);
            }
        }

        private void SetNoPropertySetState()
        {
            DataGridView.DataSource = null;
            DataGridView.Columns.Clear();
        }

        private void SetPropertySetState(ActionSetVM propertySet)
        {
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            DataGridView.DataSource = null;

            DataGridView.Columns.Clear();
            DataGridView.RowHeadersVisible = false;
            DataGridView.AllowUserToAddRows = false;
            DataGridView.AllowUserToDeleteRows = false;
            DataGridView.AllowUserToResizeRows = false;
            DataGridView.MultiSelect = false;
            DataGridView.AutoGenerateColumns = false;
            DataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridView.RowTemplate.Height = 32;

            DataGridViewCheckBoxColumn visibilityColumn = new DataGridViewCheckBoxColumn();
            visibilityColumn.HeaderText = "Visibility";
            visibilityColumn.Name = "Visibility";
            visibilityColumn.DataPropertyName = "Visibility";
            visibilityColumn.MinimumWidth = 50;
            visibilityColumn.Width = 50;
            visibilityColumn.Resizable = DataGridViewTriState.False;
            DataGridView.Columns.Add(visibilityColumn);

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
            DataGridView.Columns.Add(presentationColumn);

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.HeaderText = "Id";
            idColumn.Name = "Id";
            idColumn.DataPropertyName = "Id";
            idColumn.MinimumWidth = 40;
            idColumn.Width = 40;
            idColumn.ReadOnly = true;
            idColumn.Resizable = DataGridViewTriState.False;
            DataGridView.Columns.Add(idColumn);

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
            descriptionColumn.Resizable = DataGridViewTriState.False;
            DataGridView.Columns.Add(descriptionColumn);

            DataGridView.DataSource = _vm.CurrentItem.Items;
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Props;

namespace OpenBreed.Editor.UI.WinForms.Controls.Props
{
    public partial class PropSetEditorCtrl : UserControl
    {
        #region Private Fields

        private PropSetEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PropSetEditorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(PropSetEditorVM vm)
        {
            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            DataGridView.CurrentCellDirtyStateChanged += new EventHandler(DataGridView_CurrentCellDirtyStateChanged);
            DataGridView.SelectionChanged += new EventHandler(DataGridView_SelectionChanged);

            Update(_vm.CurrentPropSet);
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.CurrentPropSet):
                    Update(_vm.CurrentPropSet);
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

        private void SetPropertySetState(PropSetVM propertySet)
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
            //visibilityColumn.Name = m_Model.Data.Columns[0].ColumnName;
            //visibilityColumn.DataPropertyName = m_Model.Data.Columns[0].ColumnName;
            visibilityColumn.MinimumWidth = 50;
            visibilityColumn.Width = 50;
            DataGridView.Columns.Add(visibilityColumn);

            DataGridViewImageColumn presentationColumn = new DataGridViewImageColumn();
            presentationColumn.HeaderText = "Presentation";
            presentationColumn.Name = "Presentation";
            presentationColumn.DataPropertyName = "Presentation";
            //presentationColumn.Name = m_Model.Data.Columns[1].ColumnName;
            //presentationColumn.DataPropertyName = m_Model.Data.Columns[1].ColumnName;
            presentationColumn.ImageLayout = DataGridViewImageCellLayout.Normal;
            presentationColumn.DefaultCellStyle.BackColor = Color.Gray;
            presentationColumn.ReadOnly = true;
            presentationColumn.MinimumWidth = 80;
            presentationColumn.Width = 80;
            DataGridView.Columns.Add(presentationColumn);

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.HeaderText = "Id";
            idColumn.Name = "Id";
            idColumn.DataPropertyName = "Id";
            //idColumn.Name = m_Model.Data.Columns[2].ColumnName;
            //idColumn.DataPropertyName = m_Model.Data.Columns[2].ColumnName;
            idColumn.MinimumWidth = 40;
            idColumn.Width = 40;
            idColumn.ReadOnly = true;
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
            DataGridView.Columns.Add(descriptionColumn);

            DataGridView.DataSource = _vm.CurrentPropSet.Items;
            //DataGridView.DataSource = m_Model.Data;
        }

        void Update(PropSetVM propertySet)
        {
            if (propertySet == null)
                SetNoPropertySetState();
            else
                SetPropertySetState(propertySet);
        }

        #endregion Private Methods
    }
}

using OpenBreed.Editor.VM.Sprites;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteSetFromSprEditorCtrl : UserControl
    {
        #region Private Fields

        private SpriteSetFromSprEditorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromSprEditorCtrl()
        {
            InitializeComponent();

            SetupDataGridView();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(SpriteSetFromSprEditorVM vm)
        {
            this.vm = vm ?? throw new InvalidOperationException(nameof(vm));

            DGV.DataSource = vm.Items;
        }

        #endregion Public Methods

        #region Private Methods

        private void SetupDataGridView()
        {
            DGV.DataSource = null;

            DGV.Columns.Clear();
            DGV.RowHeadersVisible = false;
            DGV.AllowUserToAddRows = false;
            DGV.AllowUserToDeleteRows = false;
            DGV.AllowUserToResizeRows = true;
            DGV.MultiSelect = false;
            DGV.AutoGenerateColumns = false;
            DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGV.RowTemplate.Height = 64;

            var idColumn = new DataGridViewTextBoxColumn();
            idColumn.HeaderText = "Id";
            idColumn.Name = "Id";
            idColumn.DataPropertyName = "Id";
            idColumn.MinimumWidth = 40;
            idColumn.Width = 40;
            idColumn.ReadOnly = true;
            idColumn.Resizable = DataGridViewTriState.False;
            DGV.Columns.Add(idColumn);

            var imageColumn = new DataGridViewImageColumn();
            imageColumn.HeaderText = "Image";
            imageColumn.Name = "Image";
            imageColumn.DataPropertyName = "Image";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Normal;
            imageColumn.DefaultCellStyle.BackColor = Color.Gray;
            imageColumn.ReadOnly = true;
            imageColumn.MinimumWidth = 50;
            imageColumn.Width = 50;
            imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            imageColumn.Resizable = DataGridViewTriState.False;
            DGV.Columns.Add(imageColumn);
        }

        #endregion Private Methods
    }
}
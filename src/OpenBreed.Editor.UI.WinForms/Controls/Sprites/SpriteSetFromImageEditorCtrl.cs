using OpenBreed.Editor.VM.Sprites;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteSetFromImageEditorCtrl : UserControl
    {
        #region Private Fields

        private SpriteSetFromImageEditorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromImageEditorCtrl()
        {
            InitializeComponent();

            SetupDataGridView();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(SpriteSetFromImageEditorVM vm)
        {
            this.vm = vm ?? throw new InvalidOperationException(nameof(vm));

            SpriteEditor.Initialize(this.vm.SpriteEditor);

            btnAddSprite.Click += (s, a) => vm.AddSprite();
            btnRemoveSprite.Click += (s, a) => vm.RemoveSprite();

            DGV.DataBindings.Add(nameof(DGV.CurrentRowIndex), vm, nameof(vm.CurrentSpriteIndex), false, DataSourceUpdateMode.OnPropertyChanged);

            DGV.DataSource = vm.Items;
        }

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

        #endregion Public Methods

    }
}
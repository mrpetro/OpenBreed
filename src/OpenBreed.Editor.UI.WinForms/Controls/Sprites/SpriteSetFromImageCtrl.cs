﻿using OpenBreed.Editor.VM.Sprites;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteSetFromImageCtrl : UserControl
    {
        #region Private Fields

        private SpriteSetFromImageVM vm;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromImageCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(SpriteSetFromImageVM vm)
        {
            this.vm = vm ?? throw new InvalidOperationException(nameof(vm));

            SpriteEditor.Initialize(this.vm.SpriteEditor);

            btnAddSprite.Click += (s, a) => vm.AddSprite();
            btnRemoveSprite.Click += (s, a) => vm.RemoveSprite();

            DGV.DataBindings.Add(nameof(DGV.CurrentRowIndex), vm, nameof(vm.CurrentSpriteIndex), false, DataSourceUpdateMode.OnPropertyChanged);
            SetupDataGridView();
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

            //var descriptionColumn = new DataGridViewTextBoxColumn();
            //descriptionColumn.HeaderText = "Size";
            //descriptionColumn.Name = "Description";
            //descriptionColumn.DataPropertyName = "Description";
            //descriptionColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //descriptionColumn.ReadOnly = false;
            //descriptionColumn.Resizable = DataGridViewTriState.False;
            //DGV.Columns.Add(descriptionColumn);

            DGV.DataSource = vm.Items;
        }

        #endregion Public Methods

    }
}
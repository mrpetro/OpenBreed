﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common.Sprites;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteSetViewerCtrl : UserControl
    {
        #region Private Fields

        private SpriteSetViewerVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetViewerCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        #endregion Public Constructors


        #region Public Methods

        public void Initialize(SpriteSetViewerVM vm)
        {
            _vm = vm;

            numSpriteNo.DataBindings.Clear();
            numSpriteNo.DataBindings.Add(nameof(numSpriteNo.Value), _vm, nameof(_vm.CurrentIndex), false, DataSourceUpdateMode.OnPropertyChanged);

            _vm.PropertyChanged += _vm_PropertyChanged;
            pnlSprite.Paint += PnlSprite_Paint;

            UpdateItems();
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.CurrentSpriteSet):
                    UpdateItems();
                    break;
                case (nameof(_vm.CurrentItem)):
                    pnlSprite.Invalidate();
                    break;
                default:
                    break;
            }
        }

        private void PnlSprite_Paint(object sender, PaintEventArgs e)
        {
            if (_vm.CurrentItem != null)
                _vm.CurrentItem.Draw(e.Graphics, 0, 0, 2);
        }

        private void SetNoSpriteSetState()
        {
            this.Visible = false;
            numSpriteNo.Minimum = -1;
            numSpriteNo.Maximum = -1;
        }

        private void SetSpriteSetState()
        {
            this.Visible = true;
            numSpriteNo.Minimum = 0;
            numSpriteNo.Maximum = _vm.CurrentSpriteSet.Items.Count - 1;
        }

        void UpdateItems()
        {
            if (_vm.CurrentSpriteSet == null)
                SetNoSpriteSetState();
            else
                SetSpriteSetState();

            Invalidate();
            pnlSprite.Invalidate();
        }

        #endregion Private Methods
    }
}

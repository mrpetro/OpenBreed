﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Tiles;

namespace OpenBreed.Editor.UI.WinForms.Controls.Tiles
{
    public partial class TileSetsCtrl : UserControl
    {

        #region Private Fields

        private TileSetsVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public TileSetsCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(TileSetsVM vm)
        {
            _vm = vm;

            cbxTileSets.DataBindings.Clear();
            cbxTileSets.DataSource = _vm.Items;
            cbxTileSets.DisplayMember = "Name";

            cbxTileSets.DataBindings.Add(nameof(cbxTileSets.SelectedIndex),
                                         _vm, nameof(_vm.CurrentIndex),
                                         false, 
                                         DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods

    }
}

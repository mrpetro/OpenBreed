using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Tiles;

namespace OpenBreed.Editor.UI.WinForms.Controls.Levels
{
    public partial class LevelTileSelectorCtrl : UserControl
    {

        #region Private Fields

        private TileSetSelectorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public LevelTileSelectorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(TileSetSelectorVM vm)
        {
            _vm = vm;

            cbxTileSets.DataBindings.Clear();
            cbxTileSets.DataBindings.Add(nameof(cbxTileSets.SelectedIndex),
                                         _vm, nameof(_vm.CurrentIndex),
                                         false, 
                                         DataSourceUpdateMode.OnPropertyChanged);

            cbxTileSets.DataSource = _vm.Root.TileSets;
            cbxTileSets.DisplayMember = "Name";
        }

        #endregion Public Methods

    }
}

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
using OpenBreed.Editor.VM.Maps;

namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    public partial class MapEditorTileSetSelectorCtrl : UserControl
    {

        #region Private Fields

        private MapEditorTileSetSelectorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorTileSetSelectorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapEditorTileSetSelectorVM vm)
        {
            _vm = vm ?? throw new InvalidOperationException(nameof(vm));

            cbxTileSets.DataBindings.Clear();
            cbxTileSets.DataBindings.Add(nameof(cbxTileSets.SelectedIndex),
                                         _vm, nameof(_vm.CurrentIndex),
                                         false,
                                         DataSourceUpdateMode.OnPropertyChanged);

            cbxTileSets.DataSource = _vm.TileSets;
            cbxTileSets.DisplayMember = "Id";
        }

        #endregion Public Methods

    }
}

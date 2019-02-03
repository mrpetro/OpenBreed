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
using OpenBreed.Editor.VM.Levels;

namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    public partial class MapEditorTilesToolCtrl : UserControl
    {

        #region Private Fields

        private MapEditorTilesToolVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorTilesToolCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapEditorTilesToolVM vm)
        {
            _vm = vm;

            cbxTileSets.DataBindings.Clear();
            cbxTileSets.DataBindings.Add(nameof(cbxTileSets.SelectedIndex),
                                         _vm, nameof(_vm.CurrentIndex),
                                         false,
                                         DataSourceUpdateMode.OnPropertyChanged);

            //cbxTileSets.DataSource = _vm.Parent.Root.LevelEditor.CurrentLevel.TileSets;
            cbxTileSets.DisplayMember = "Name";
        }

        #endregion Public Methods

    }
}

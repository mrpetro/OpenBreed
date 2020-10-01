using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Maps;

namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    public partial class MapEditorPalettesToolCtrl : UserControl
    {
        #region Private Fields

        private MapEditorPalettesToolVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorPalettesToolCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapEditorPalettesToolVM vm)
        {
            _vm = vm;

            cbxPalettes.DataBindings.Clear();

            //TODO: Make that work again
            //cbxPalettes.DataSource = _vm.Parent.Palettes;
            cbxPalettes.DisplayMember = "Name";

            cbxPalettes.DataBindings.Add("SelectedIndex", _vm, nameof(_vm.CurrentIndex), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods

    }
}

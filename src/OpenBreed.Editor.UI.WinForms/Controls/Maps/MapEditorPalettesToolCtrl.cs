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

        private MapEditorPalettesToolVM vm;

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
            this.vm = vm;
            BindControls();
        }

        private void BindControls()
        {
            cbxPalettes.DataBindings.Clear();
            cbxPalettes.DataSource = vm.PaletteNames;
            cbxPalettes.DisplayMember = "Name";
            cbxPalettes.DataBindings.Add(nameof(cbxPalettes.SelectedItem), vm, nameof(vm.CurrentPaletteRef), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods

    }
}

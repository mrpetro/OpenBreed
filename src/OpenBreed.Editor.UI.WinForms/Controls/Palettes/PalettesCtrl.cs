using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Common.Palettes;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM.Palettes;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PalettesCtrl : UserControl
    {
        #region Private Fields

        private PalettesVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PalettesCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(PalettesVM vm)
        {
            _vm = vm;

            cbxPalettes.DataBindings.Clear();
            cbxPalettes.DataSource = _vm.Items;
            cbxPalettes.DisplayMember = "Name";

            cbxPalettes.DataBindings.Add("SelectedIndex", _vm, nameof(_vm.CurrentIndex), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods

    }
}

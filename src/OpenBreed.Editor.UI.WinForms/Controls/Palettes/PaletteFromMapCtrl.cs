using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.UI.WinForms.Helpers;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PaletteFromMapCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private PaletteFromMapEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromMapCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(PaletteFromMapEditorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            var colorEditor = WpfHelper.CreateWpfControl<Wpf.Palettes.ColorEditorCtrl>(_vm);
            colorEditor.Dock = DockStyle.Top;
            grpPalette.Controls.Add(colorEditor);

            //ColorEditor.Initialize(_vm);
            ColorSelector.Initialize(_vm);

            tbxMapDataRef.DataBindings.Add(nameof(tbxMapDataRef.Text), _vm, nameof(_vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged);

            cbxMapBlockName.DataSource = _vm.BlockNames;
            cbxMapBlockName.DataBindings.Add(nameof(cbxMapBlockName.Text), _vm, nameof(_vm.BlockName), false, DataSourceUpdateMode.OnPropertyChanged);

            colorEditor.DataBindings.Add(nameof(colorEditor.Enabled), _vm, nameof(_vm.EditEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            ColorSelector.DataBindings.Add(nameof(ColorSelector.Enabled), _vm, nameof(_vm.EditEnabled), false, DataSourceUpdateMode.OnPropertyChanged);


        }

        #endregion Public Methods
    }
}

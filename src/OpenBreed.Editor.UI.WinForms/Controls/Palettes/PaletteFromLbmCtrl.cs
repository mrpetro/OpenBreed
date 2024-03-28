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
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PaletteFromLbmCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private PaletteFromLbmEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromLbmCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as PaletteFromLbmEditorVM ?? throw new ArgumentNullException(nameof(vm));

            var colorSelector = WpfHelper.CreateWpfControl<Wpf.Palettes.ColorSelectorCtrl>(_vm);
            colorSelector.Dock = DockStyle.Fill;
            grpPalette.Controls.Add(colorSelector);

            var colorEditor = WpfHelper.CreateWpfControl<Wpf.Palettes.ColorEditorCtrl>(_vm.ColorEditor);
            colorEditor.Dock = DockStyle.Top;
            grpPalette.Controls.Add(colorEditor);

            tbxLbmDataRef.DataBindings.Add(nameof(tbxLbmDataRef.Text), _vm, nameof(_vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

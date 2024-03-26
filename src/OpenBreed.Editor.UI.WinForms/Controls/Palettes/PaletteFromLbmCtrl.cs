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

        public void Initialize(PaletteFromLbmEditorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            var colorEditor = WpfHelper.CreateWpfControl<Wpf.Palettes.ColorEditorCtrl>(_vm);
            colorEditor.Dock = DockStyle.Top;
            grpPalette.Controls.Add(colorEditor);

            ColorSelector.Initialize(_vm);

            tbxLbmDataRef.DataBindings.Add(nameof(tbxLbmDataRef.Text), _vm, nameof(_vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged);

            ColorSelector.DataBindings.Add(nameof(ColorSelector.Enabled), _vm, nameof(_vm.EditEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

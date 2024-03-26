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
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.UI.WinForms.Helpers;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PaletteFromBinaryCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private PaletteFromBinaryEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromBinaryCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(PaletteFromBinaryEditorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            var colorEditor = WpfHelper.CreateWpfControl<Wpf.Palettes.ColorEditorCtrl>(_vm);
            colorEditor.Dock = DockStyle.Top;
            grpPalette.Controls.Add(colorEditor);

            ColorSelector.Initialize(_vm);

            tbxBinaryDataRef.DataBindings.Add(nameof(tbxBinaryDataRef.Text), _vm, nameof(_vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged);

            numUpDown.DataBindings.Add(nameof(numUpDown.Value), _vm, nameof(_vm.DataStart), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

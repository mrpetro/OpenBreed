using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Assets;
using OpenBreed.Editor.VM.Palettes;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PaletteFromMapCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private PaletteFromMapVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromMapCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(PaletteFromMapVM vm)
        {
            _vm = vm ?? throw new InvalidOperationException(nameof(vm));

            ColorEditor.Initialize(_vm);
            ColorSelector.Initialize(_vm);

            tbxMapDataRef.DataBindings.Add(nameof(tbxMapDataRef.Text), _vm, nameof(_vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxMapBlockName.DataBindings.Add(nameof(cbxMapBlockName.Text), _vm, nameof(_vm.BlockName), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

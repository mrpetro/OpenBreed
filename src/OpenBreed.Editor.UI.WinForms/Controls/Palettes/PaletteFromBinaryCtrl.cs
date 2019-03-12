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
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Palettes;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PaletteFromBinaryCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private PaletteFromBinaryVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromBinaryCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(PaletteFromBinaryVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            ColorEditor.Initialize(_vm);
            ColorSelector.Initialize(_vm);

            tbxBinaryDataRef.DataBindings.Add(nameof(tbxBinaryDataRef.Text), _vm, nameof(_vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged); 
        }

        #endregion Public Methods
    }
}

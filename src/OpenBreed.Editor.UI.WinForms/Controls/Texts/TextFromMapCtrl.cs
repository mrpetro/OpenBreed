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
using OpenBreed.Editor.VM.Texts;

namespace OpenBreed.Editor.UI.WinForms.Controls.Texts
{
    public partial class TextFromMapCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private TextFromMapVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public TextFromMapCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(TextFromMapVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            tbxMapDataRef.DataBindings.Add(nameof(tbxMapDataRef.Text), _vm, nameof(_vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged);

            cbxMapBlockName.DataSource = _vm.BlockNames;
            cbxMapBlockName.DataBindings.Add(nameof(cbxMapBlockName.Text), _vm, nameof(_vm.BlockName), false, DataSourceUpdateMode.OnPropertyChanged);

            tbxText.DataBindings.Add(nameof(tbxText.Text), _vm, nameof(_vm.Text), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxText.DataBindings.Add(nameof(tbxText.Enabled), _vm, nameof(_vm.EditEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

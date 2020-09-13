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
    public partial class TextFromMapEditorCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private TextFromMapEditorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public TextFromMapEditorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(TextFromMapEditorVM vm)
        {
            this.vm = vm ?? throw new ArgumentNullException(nameof(vm));

            tbxMapDataRef.DataBindings.Add(nameof(tbxMapDataRef.Text), this.vm, nameof(vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged);

            cbxMapBlockName.DataSource = this.vm.BlockNames;
            cbxMapBlockName.DataBindings.Add(nameof(cbxMapBlockName.Text), this.vm, nameof(vm.BlockName), false, DataSourceUpdateMode.OnPropertyChanged);

            tbxText.DataBindings.Add(nameof(tbxText.Text), this.vm, nameof(vm.Text), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxText.DataBindings.Add(nameof(tbxText.Enabled), this.vm, nameof(vm.EditEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

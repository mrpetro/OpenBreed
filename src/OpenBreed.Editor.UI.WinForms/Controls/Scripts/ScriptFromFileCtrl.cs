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
using OpenBreed.Editor.VM.Scripts;

namespace OpenBreed.Editor.UI.WinForms.Controls.Scripts
{
    public partial class ScriptFromFileCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private ScriptFromFileVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public ScriptFromFileCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(ScriptFromFileVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            ScriptAssetRefIdEditor.Initialize(_vm.ScriptAssetRefIdEditor);

            tbxText.DataBindings.Add(nameof(tbxText.Text), _vm, nameof(_vm.Script), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxText.DataBindings.Add(nameof(tbxText.Enabled), _vm, nameof(_vm.EditEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

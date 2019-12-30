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
using OpenBreed.Editor.VM.Texts;

namespace OpenBreed.Editor.UI.WinForms.Controls. Texts
{
    public partial class TextEmbeddedCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private TextEmbeddedVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public TextEmbeddedCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(TextEmbeddedVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            tbxText.DataBindings.Add(nameof(tbxText.Text), _vm, nameof(_vm.Text), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

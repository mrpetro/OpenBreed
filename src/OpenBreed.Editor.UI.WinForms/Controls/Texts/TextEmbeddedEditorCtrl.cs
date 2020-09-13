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
    public partial class TextEmbeddedEditorCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private TextEmbeddedEditorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public TextEmbeddedEditorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(TextEmbeddedEditorVM vm)
        {
            this.vm = vm ?? throw new ArgumentNullException(nameof(vm));

            tbxText.DataBindings.Add(nameof(tbxText.Text), this.vm, nameof(vm.Text), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Sounds;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class SoundEditorView : DockContent
    {
        #region Private Fields

        private SoundEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public SoundEditorView()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(SoundEditorVM vm)
        {
            _vm = vm;

            SoundEditor.Initialize(_vm);

            _vm.PropertyChanged += _vm_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion Private Methods
    }
}

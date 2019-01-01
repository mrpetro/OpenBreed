using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Sounds;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sounds
{
    public partial class SoundEditorCtrl : UserControl
    {
        private SoundEditorVM _vm;

        public SoundEditorCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        public void Initialize(SoundEditorVM vm)
        {
            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            UpdateViewState();
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.EditableSound):
                    UpdateViewState();
                    break;
                default:
                    break;
            }
        }

        private void UpdateViewState()
        {
            if (_vm.EditableSound == null)
                SetNoEditableState();
            else
                SetEditableState();
        }

        private void SetNoEditableState()
        {
            Invalidate();
        }

        private void SetEditableState()
        {
            Invalidate();
        }

        private void btnPlaySound_Click(object sender, EventArgs e)
        {
            _vm.Play();
        }
    }
}

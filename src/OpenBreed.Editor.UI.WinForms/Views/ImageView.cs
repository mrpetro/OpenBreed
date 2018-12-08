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

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class ImageView : DockContent
    {
        #region Private Fields

        private ImageViewerVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public ImageView()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(ImageViewerVM vm)
        {
            _vm = vm;

            ImageViewer.Initialize(_vm);

            _vm.PropertyChanged += _vm_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Image):
                    if (_vm.Image == null)
                        return;
                    Width = _vm.Image.Width;
                    Height = _vm.Image.Height;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}

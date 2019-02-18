using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Images;

namespace OpenBreed.Editor.UI.WinForms.Controls.Images
{
    public partial class ImageDataSourceCtrl : UserControl
    {
        #region Private Fields

        private ImageDataSourceVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public ImageDataSourceCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(ImageDataSourceVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(ImageDataSourceVM));

            AssetEntryRef.Initialize(vm.AssetEntryRef);
        }

        #endregion Public Methods
    }
}

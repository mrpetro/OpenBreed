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

namespace OpenBreed.Editor.UI.WinForms.Controls.Assets
{
    public partial class FileAssetCtrl : UserControl
    {
        #region Private Fields

        private FileAssetVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public FileAssetCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(FileAssetVM vm)
        {
            _vm = vm;

            tbxFilePath.DataBindings.Add(nameof(tbxFilePath.Text), _vm, nameof(_vm.FilePath), false, DataSourceUpdateMode.OnPropertyChanged); 
        }

        #endregion Public Methods
    }
}

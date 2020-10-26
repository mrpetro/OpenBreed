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

namespace OpenBreed.Editor.UI.WinForms.Controls.DataSources
{
    public partial class FileDataSourceCtrl : UserControl
    {
        #region Private Fields

        private FileDataSourceEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public FileDataSourceCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(FileDataSourceEditorVM vm)
        {
            _vm = vm;

            tbxFilePath.DataBindings.Add(nameof(tbxFilePath.Text), _vm, nameof(_vm.FilePath), false, DataSourceUpdateMode.OnPropertyChanged); 
        }

        #endregion Public Methods
    }
}

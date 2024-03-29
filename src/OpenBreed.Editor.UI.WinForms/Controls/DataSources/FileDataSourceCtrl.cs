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

namespace OpenBreed.Editor.UI.WinForms.Controls.DataSources
{
    public partial class FileDataSourceCtrl : EntryEditorInnerCtrl
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

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as FileDataSourceEditorVM ?? throw new ArgumentNullException(nameof(vm));

            tbxFilePath.DataBindings.Add(nameof(tbxFilePath.Text), _vm, nameof(_vm.FilePath), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}
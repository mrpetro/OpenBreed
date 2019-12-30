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
    public partial class EpfArchiveDataSourceCtrl : UserControl
    {
        #region Private Fields

        private EPFArchiveFileDataSourceVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public EpfArchiveDataSourceCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(EPFArchiveFileDataSourceVM vm)
        {
            _vm = vm;

            tbxEpfArchivePath.DataBindings.Add(nameof(tbxEpfArchivePath.Text), _vm, nameof(_vm.ArchivePath), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxEntryName.DataBindings.Add(nameof(cbxEntryName.Text), _vm, nameof(_vm.EntryName), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

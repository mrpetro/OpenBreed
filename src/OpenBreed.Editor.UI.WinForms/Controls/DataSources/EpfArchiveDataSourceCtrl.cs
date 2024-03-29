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
    public partial class EpfArchiveDataSourceCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private EpfArchiveFileDataSourceEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public EpfArchiveDataSourceCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as EpfArchiveFileDataSourceEditorVM ?? throw new ArgumentNullException(nameof(vm));

            tbxEpfArchivePath.DataBindings.Add(nameof(tbxEpfArchivePath.Text), _vm, nameof(_vm.ArchivePath), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxEntryName.DataBindings.Add(nameof(cbxEntryName.Text), _vm, nameof(_vm.EntryName), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}
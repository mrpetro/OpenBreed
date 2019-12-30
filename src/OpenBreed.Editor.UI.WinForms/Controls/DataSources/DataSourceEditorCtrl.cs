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
    public partial class DataSourceEditorCtrl : EntryEditorInnerCtrl
    {
        private DataSourceEditorVM _vm;

        public DataSourceEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as DataSourceEditorVM ?? throw new InvalidOperationException(nameof(vm));

            _vm.PropertyChanged += _vm_PropertyChanged;

            OnEditableChanged(_vm.Editable);
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Editable):
                    OnEditableChanged(_vm.Editable);
                    break;
                default:
                    break;
            }
        }

        private void OnEditableChanged(DataSourceVM asset)
        {
            Controls.Clear();

            if (asset == null)
                return;

            if (asset is FileDataSourceVM)
            {
                var control = new FileDataSourceCtrl();
                control.Initialize((FileDataSourceVM)asset);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (asset is EPFArchiveFileDataSourceVM)
            {
                var control = new EpfArchiveDataSourceCtrl();
                control.Initialize((EPFArchiveFileDataSourceVM)asset);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }
    }
}

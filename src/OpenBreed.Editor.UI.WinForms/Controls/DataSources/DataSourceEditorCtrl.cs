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
using OpenBreed.Database.Interface.Items.DataSources;

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
            OnSubeditorChanged(_vm.Subeditor);
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Subeditor):
                    OnSubeditorChanged(_vm.Subeditor);
                    break;
                default:
                    break;
            }
        }

        private void OnSubeditorChanged(IEntryEditor<IDataSourceEntry> subeditor)
        {
            Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is EpfArchiveFileDataSourceEditorVM)
            {
                var control = new EpfArchiveDataSourceCtrl();
                control.Initialize((EpfArchiveFileDataSourceEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (subeditor is FileDataSourceEditorVM)
            {
                var control = new FileDataSourceCtrl();
                control.Initialize((FileDataSourceEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }
    }
}

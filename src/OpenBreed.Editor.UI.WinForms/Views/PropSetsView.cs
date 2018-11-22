using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing.Imaging;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Common.Props;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class PropSetsView : DockContent
    {
        private PropSetsVM _vm;

        public PropSetsView()
        {
            InitializeComponent();

        }

        public void Initialize(PropSetsVM vm)
        {
            _vm = vm;

            PropSets.Initialize(vm);
            PropSelector.Initialize(vm.PropSelector);

            _vm.PropertyChanged += _vm_PropertyChanged;

            TabText = _vm.Title;
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Title):
                    TabText = _vm.Title;
                    break;
                default:
                    break;
            }
        }
    }
}

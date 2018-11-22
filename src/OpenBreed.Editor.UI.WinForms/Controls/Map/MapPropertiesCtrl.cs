using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Maps;

namespace OpenBreed.Editor.UI.WinForms.Controls.Map
{
    public partial class MapPropertiesCtrl : UserControl
    {
        #region Private Fields

        private MapPropertiesVM _vm;

        #endregion Private Fields

        public MapPropertiesCtrl()
        {
            InitializeComponent();
        }

        public void Initialize(MapPropertiesVM vm)
        {
            _vm = vm;

            tbxXBLK.DataBindings.Add("Text", _vm, nameof(_vm.XBLK), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxYBLK.DataBindings.Add("Text", _vm, nameof(_vm.YBLK), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxXOFC.DataBindings.Add("Text", _vm, nameof(_vm.XOFC), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxYOFC.DataBindings.Add("Text", _vm, nameof(_vm.YOFC), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxXOFM.DataBindings.Add("Text", _vm, nameof(_vm.XOFM), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxYOFM.DataBindings.Add("Text", _vm, nameof(_vm.YOFM), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxXOFA.DataBindings.Add("Text", _vm, nameof(_vm.XOFA), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxYOFA.DataBindings.Add("Text", _vm, nameof(_vm.YOFA), false, DataSourceUpdateMode.OnPropertyChanged);

            //_vm.PropertyChanged += _vm_PropertyChanged;
        }
    }
}

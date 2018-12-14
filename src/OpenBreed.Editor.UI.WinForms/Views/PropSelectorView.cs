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
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class PropSelectorView : DockContent
    {
        #region Private Fields

        private PropSelectorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PropSelectorView()
        {
            InitializeComponent();

        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(PropSelectorVM vm)
        {
            _vm = vm;

            PropSelector.Initialize(_vm);

            _vm.PropertyChanged += _vm_PropertyChanged;

            TabText = _vm.Title;
        }

        #endregion Public Methods

        #region Private Methods

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

        #endregion Private Methods
    }
}

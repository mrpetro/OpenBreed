using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Props;

namespace OpenBreed.Editor.UI.WinForms.Controls.Props
{
    public partial class PropSetsCtrl : UserControl
    {

        #region Private Fields

        private PropSetsVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PropSetsCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(PropSetsVM vm)
        {
            _vm = vm;

            Update(_vm.CurrentItem);
        }

        private void SetNoPropertySetState()
        {
            cbxPropSets.DataBindings.Clear();
            cbxPropSets.Text = null;
            cbxPropSets.DataSource = null;
        }

        private void SetPropertySetState(PropSetVM propertySet)
        {
            cbxPropSets.DataBindings.Clear();
            cbxPropSets.DataSource = _vm.Items;
            cbxPropSets.DisplayMember = "Name";

            cbxPropSets.DataBindings.Add(nameof(cbxPropSets.SelectedIndex),
                                         _vm, nameof(_vm.CurrentIndex),
                                         false,
                                         DataSourceUpdateMode.OnPropertyChanged);
        }

        void Update(PropSetVM propertySet)
        {
            if (propertySet == null)
                SetNoPropertySetState();
            else
                SetPropertySetState(propertySet);
        }


        #endregion Public Methods

    }
}

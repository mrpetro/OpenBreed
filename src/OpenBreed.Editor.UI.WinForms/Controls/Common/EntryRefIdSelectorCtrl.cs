using OpenBreed.Editor.VM.Common;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Controls.Common
{
    public partial class EntryRefIdSelectorCtrl : UserControl
    {
        #region Private Fields

        private EntryRefIdSelectorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public EntryRefIdSelectorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(EntryRefIdSelectorVM vm)
        {
            this.vm = vm;

            BindControls();
        }

        #endregion Public Methods

        #region Private Methods

        private void BindControls()
        {
            cbxEntryItems.DataSource = vm.Items;

            cbxEntryItems.DataBindings.Add(nameof(cbxEntryItems.SelectedItem),
                                           vm,
                                           nameof(vm.CurrentEntryId),
                                           false,
                                           DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Private Methods
    }
}
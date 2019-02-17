using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Database;

namespace OpenBreed.Editor.UI.WinForms.Controls.Database
{
    public partial class DbTableNewEntryCreatorCtrl : UserControl
    {
        #region Private Fields

        private DbTableNewEntryCreatorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public DbTableNewEntryCreatorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(DbTableNewEntryCreatorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(DbTableNewEntryCreatorVM));

            tbxEntryId.DataBindings.Add(nameof(tbxEntryId.Text), _vm, nameof(_vm.NewId), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxEntryType.DataSource = _vm.EntryTypes;
            cbxEntryType.DisplayMember = "Name";
            cbxEntryType.DataBindings.Add(nameof(cbxEntryType.SelectedItem), _vm, nameof(_vm.EntryType), false, DataSourceUpdateMode.OnPropertyChanged);
            btnCreate.DataBindings.Add(nameof(btnCreate.Enabled), _vm, nameof(_vm.CreateEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            btnCreate.Click += (s, a) => _vm.Create();
            btnCancel.Click += (s, a) => _vm.Close();

            tbxEntryId.Focus();
        }

        #endregion Public Methods
    }
}

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

        private DbTableNewEntryCreatorVM vm;

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
            this.vm = vm ?? throw new ArgumentNullException(nameof(DbTableNewEntryCreatorVM));

            BindControls();
            BindEvents();

            tbxEntryId.Focus();
        }

        private void BindEvents()
        {
            btnCreate.Click += (s, a) => vm.Create();
            btnCancel.Click += (s, a) => vm.Close();
        }

        private void BindControls()
        {
            tbxEntryId.DataBindings.Add(nameof(tbxEntryId.Text), vm, nameof(vm.NewId), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxEntryType.DataSource = vm.EntryTypes;
            cbxEntryType.DisplayMember = "Name";
            cbxEntryType.DataBindings.Add(nameof(cbxEntryType.SelectedItem), vm, nameof(vm.EntryType), false, DataSourceUpdateMode.OnPropertyChanged);
            btnCreate.DataBindings.Add(nameof(btnCreate.Enabled), vm, nameof(vm.CreateEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}

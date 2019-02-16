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

            tbxId.DataBindings.Add(nameof(tbxId.Text), _vm, nameof(_vm.NewId), false, DataSourceUpdateMode.OnPropertyChanged);
            btnCreate.DataBindings.Add(nameof(btnCreate.Enabled), _vm, nameof(_vm.CreateEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            btnCreate.Click += (s, a) => _vm.Create();
            btnCancel.Click += (s, a) => _vm.Close();

            tbxId.Focus();
        }

        #endregion Public Methods
    }
}

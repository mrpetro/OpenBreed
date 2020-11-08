using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OpenBreed.Editor.VM.Database;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class DbTablesEditorView : DockContent
    {
        public DbTablesEditorView()
        {
            InitializeComponent();
        }

        public void Initialize(DbTablesEditorVM vm)
        {
            DbTablesEditorCtrl.Initialize(vm);

            vm.ShowingAction = () => this.InvokeIfRequired(() => base.Show());
            vm.HidingAction = () => this.InvokeIfRequired(() => base.Hide());
            vm.ClosingAction = () => this.InvokeIfRequired(() => base.Close());
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);


            //VM.ClosingAction();
            //if (e.CloseReason == CloseReason.UserClosing)
            //    e.Cancel = !VM.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            //VM.ClosedAction();
        }
    }
}

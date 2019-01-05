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
            DatabaseViewer.Initialize(vm);
        }
    }
}

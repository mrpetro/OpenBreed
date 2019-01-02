using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Controls
{
    public partial class EntryEditorInnerCtrl : UserControl
    {
        private EntryEditorVM _vm;

        public EntryEditorInnerCtrl()
        {
            InitializeComponent();
        }

        public virtual void Initialize(EntryEditorVM vm)
        {
            throw new NotImplementedException();
        }
    }
}

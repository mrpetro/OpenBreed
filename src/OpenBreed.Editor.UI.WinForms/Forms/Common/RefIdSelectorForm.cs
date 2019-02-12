using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Common;

namespace OpenBreed.Editor.UI.WinForms.Forms.Common
{
    public partial class RefIdSelectorForm : Form
    {
        private RefSelectorVM _vm;

        public RefIdSelectorForm()
        {
            InitializeComponent();
        }

        internal void Initialize(RefSelectorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(RefSelectorVM));

            Ctrl.Initialize(vm);
        }
    }
}

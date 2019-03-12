using OpenBreed.Editor.VM.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class LogConsoleView : DockContent
    {
        private LoggerVM _vm;

        public LogConsoleView()
        {
            InitializeComponent();
        }

        public void Initialize(LoggerVM vm)
        {
            _vm = vm ?? throw new InvalidOperationException(nameof(vm));


        }
    }
}

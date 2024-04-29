using OpenBreed.Editor.VM.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class LogConsoleView : UserControl
    {
        private LoggerVM vm;

        public LogConsoleView()
        {
            InitializeComponent();
        }

        public void Initialize(LoggerVM vm)
        {
            this.vm = vm ?? throw new InvalidOperationException(nameof(vm));

            LogConsoleCtrl.Initialize(vm.logger);
        }
    }
}

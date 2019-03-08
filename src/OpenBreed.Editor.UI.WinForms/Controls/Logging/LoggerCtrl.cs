using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Logging;

namespace OpenBreed.Editor.UI.WinForms.Controls.Logging
{
    public partial class LoggerCtrl : UserControl
    {
        #region Private Fields

        private LoggerVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public LoggerCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(LoggerVM vm)
        {
            _vm = vm ?? throw new InvalidOperationException(nameof(vm));

        }

        #endregion Public Methods
    }
}

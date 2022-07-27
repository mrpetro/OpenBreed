using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Interface.Logging;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    public partial class LogConsoleCtrl : UserControl
    {
        private ILogger logger;

        private Color m_VerboseColor = Color.Gray;
        private Color m_InfoColor = Color.Black;
        private Color m_WarningColor = Color.Orange;
        private Color m_ErrorColor = Color.Red;
        private Color m_CriticalColor = Color.DarkRed;

        public LogConsoleCtrl()
        {
            InitializeComponent();


            Disposed += new EventHandler(LogConsoleCtrl_Disposed);
        }

        public void Initialize(ILogger logger)
        {
            this.logger = logger;
            this.logger.MessageAdded += LogMan_MessageAdded;
        }

        void LogConsoleCtrl_Disposed(object sender, EventArgs e)
        {
            if(logger != null)
                logger.MessageAdded -= LogMan_MessageAdded;
        }

        void LogMan_MessageAdded(LogLevel level, int channel, string messageText)
        {
            switch (level)
            {
                case LogLevel.Verbose:
                    PrintVerboseMessage(messageText);
                    break;
                case LogLevel.Info:
                    PrintInfoMessage(messageText);
                    break;
                case LogLevel.Warning:
                    PrintWarningMessage(messageText);
                    break;
                case LogLevel.Error:
                    PrintErrorMessage(messageText);
                    break;
                case LogLevel.Critical:
                    PrintCriticalMessage(messageText);
                    break;
                default:
                    break;
            }
        }

        void PrintVerboseMessage(string messageText)
        {
            tbxConsole.SelectionColor = m_VerboseColor;
            tbxConsole.SelectionBackColor = tbxConsole.BackColor;
            tbxConsole.AppendText(messageText + "\n");
        }

        void PrintInfoMessage(string messageText)
        {
            tbxConsole.SelectionColor = m_InfoColor;
            tbxConsole.SelectionBackColor = tbxConsole.BackColor;
            tbxConsole.AppendText(messageText + "\n");
        }

        void PrintWarningMessage(string messageText)
        {
            tbxConsole.SelectionColor = m_WarningColor;
            tbxConsole.SelectionBackColor = tbxConsole.BackColor;
            tbxConsole.AppendText(messageText + "\n");
        }

        void PrintErrorMessage(string messageText)
        {
            tbxConsole.SelectionColor = m_ErrorColor;
            tbxConsole.SelectionBackColor = tbxConsole.BackColor;
            tbxConsole.AppendText(messageText + "\n");
        }

        void PrintCriticalMessage(string messageText)
        {
            tbxConsole.SelectionColor = tbxConsole.BackColor;
            tbxConsole.SelectionBackColor = m_ErrorColor;
            tbxConsole.AppendText(messageText + "\n");
        }
    }
}

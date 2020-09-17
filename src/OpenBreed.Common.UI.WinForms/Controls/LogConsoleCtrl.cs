using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Common.Logging;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    public partial class LogConsoleCtrl : UserControl
    {
        private ILogger logger;

        private Color m_SuccessColor = Color.Green;
        private Color m_WarningColor = Color.Orange;
        private Color m_DebugColor = Color.Gray;
        private Color m_InfoColor = Color.Black;
        private Color m_ErrorColor = Color.Red;

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

        void LogMan_MessageAdded(LogType type, string messageText)
        {
            switch (type)
            {
                case LogType.Error:
                    PrintErrorMessage(messageText);
                    break;
                case LogType.Warning:
                    PrintWarningMessage(messageText);
                    break;
                case LogType.Info:
                    PrintInfoMessage(messageText);
                    break;
                case LogType.Success:
                    PrintSuccessMessage(messageText);
                    break;
                case LogType.Debug:
                    PrintDebugMessage(messageText);
                    break;
                case LogType.None:
                    break;
                default:
                    break;
            }
        }

        void PrintErrorMessage(string messageText)
        {
            tbxConsole.SelectionColor = m_ErrorColor;
            tbxConsole.AppendText(messageText + "\n");
        }

        void PrintWarningMessage(string messageText)
        {
            tbxConsole.SelectionColor = m_WarningColor;
            tbxConsole.AppendText(messageText + "\n");
        }

        void PrintSuccessMessage(string messageText)
        {
            tbxConsole.SelectionColor = m_SuccessColor;
            tbxConsole.AppendText(messageText + "\n");
        }

        void PrintInfoMessage(string messageText)
        {
            tbxConsole.SelectionColor = m_InfoColor;
            tbxConsole.AppendText(messageText + "\n");
        }

        void PrintDebugMessage(string messageText)
        {
            tbxConsole.SelectionColor = m_DebugColor;
            tbxConsole.AppendText(messageText + "\n");
        }


    }
}

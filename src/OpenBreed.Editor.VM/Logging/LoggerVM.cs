using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Logging
{
    public class LoggerVM : BaseViewModel
    {
        #region Private Fields

        private readonly ILoggerClient loggerClient;

        #endregion Private Fields

        #region Public Constructors

        public LoggerVM(ILoggerClient loggerClient)
        {
            this.loggerClient = loggerClient;
            Logs = new ObservableCollection<LogItemVM>();

            loggerClient.MessageAdded += LoggerClient_MessageAdded;

            ClearCommand = new Command(() => Logs.Clear());

        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand ClearCommand { get; }

        public ObservableCollection<LogItemVM> Logs { get; }

        #endregion Public Properties

        #region Private Methods

        private void LoggerClient_MessageAdded(LogLevel type, string msg)
        {
            Logs.Insert(0, new LogItemVM(msg, type.ToString()));
        }

        #endregion Private Methods
    }
}
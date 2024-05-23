using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
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

        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public LoggerVM(ILogger logger)
        {
            this.logger = logger;

            logger.MessageAdded += Instance_MessageAdded;
            Logs = new ObservableCollection<LogItemVM>();

            ClearCommand = new Command(() => Logs.Clear());
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand ClearCommand { get; }

        public ObservableCollection<LogItemVM> Logs { get; }

        #endregion Public Properties

        #region Private Methods

        private void Instance_MessageAdded(LogLevel level, int channel, string msg)
        {
            Logs.Insert(0, new LogItemVM(msg, level.ToString()));
        }

        #endregion Private Methods
    }
}
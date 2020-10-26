using OpenBreed.Common.Logging;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Logging
{
    public class LoggerVM : BaseViewModel
    {
        #region Public Fields
        private ILogger logger;
        public BindingList<LogItemVM> Items;

        #endregion Public Fields

        #region Public Constructors

        public LoggerVM(ILogger logger)
        {
            this.logger = logger;

            logger.MessageAdded += Instance_MessageAdded;
            Items = new BindingList<LogItemVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Public Constructors

        #region Public Methods

        public static Color GetMessageTypeColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Verbose:
                    return Color.Gray;
                case LogLevel.Info:
                    return Color.Black;
                case LogLevel.Warning:
                    return Color.Gold;
                case LogLevel.Error:
                    return Color.Red;
                case LogLevel.Critical:
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Instance_MessageAdded(LogLevel level, string msg)
        {
            Items.Add(new LogItemVM()
            {
                Color = GetMessageTypeColor(level),
                MessageText = msg,
                MessageType = level.ToString()
            });
        }

        #endregion Private Methods
    }
}

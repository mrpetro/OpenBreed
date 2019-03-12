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

        public BindingList<LogItemVM> Items;

        #endregion Public Fields

        #region Public Constructors

        public LoggerVM()
        {
            LogMan.Instance.MessageAdded += Instance_MessageAdded;

            Items = new BindingList<LogItemVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Public Constructors

        #region Public Methods

        public static Color GetMessageTypeColor(LogType type)
        {
            switch (type)
            {
                case LogType.Critical:
                    return Color.Red;
                case LogType.Error:
                    return Color.Red;
                case LogType.Warning:
                    return Color.Gold;
                case LogType.Info:
                    return Color.Black;
                case LogType.Success:
                    return Color.Green;
                case LogType.Debug:
                    return Color.Gray;
                case LogType.None:
                    return Color.Black;
                default:
                    return Color.Black;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Instance_MessageAdded(LogType type, string msg)
        {
            Items.Add(new LogItemVM()
            {
                Color = GetMessageTypeColor(type),
                MessageText = msg,
                MessageType = type.ToString()
            });
        }

        #endregion Private Methods
    }
}

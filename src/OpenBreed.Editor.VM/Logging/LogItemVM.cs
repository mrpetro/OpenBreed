using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Logging
{
    public class LogItemVM
    {
        #region Public Constructors

        public LogItemVM(string message, string level)
        {
            Message = message;
            Level = level;
        }

        #endregion Public Constructors

        #region Public Properties

        public Color Color { get; }
        public string Message { get; }
        public string Level { get; }

        #endregion Public Properties
    }
}
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
        #region Public Properties

        public Color Color { get; set; }
        public string MessageText { get; set; }
        public string MessageType { get; set; }

        #endregion Public Properties
    }
}

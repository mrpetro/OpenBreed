using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing.Wrappers
{
    public class FontWrapper : IFont
    {
        #region Public Constructors

        public FontWrapper(Font wrapped)
        {
            Wrapped = wrapped;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name => Wrapped.Name;

        public float Size => Wrapped.Size;

        public Font Wrapped { get; }

        #endregion Public Properties
    }
}

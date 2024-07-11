using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing.Wrappers
{
    public class SolidBrushWrapper : IBrush
    {
        #region Public Constructors

        public SolidBrushWrapper(SolidBrush wrapped)
        {
            Wrapped = wrapped;
        }

        #endregion Public Constructors

        #region Public Properties

        public MyColor Color
        {
            get
            {
                var c = Wrapped.Color;
                return MyColor.FromArgb(c.A, c.R, c.G, c.B);
            }

            set
            {
                Wrapped.Color = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
            }
        }

        public SolidBrush Wrapped { get; }

        #endregion Public Properties
    }
}

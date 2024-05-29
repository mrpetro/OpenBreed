using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Windows.Drawing.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing
{
    public class PensProvider : IPensProvider
    {
        public PensProvider()
        {
            Red = new PenWrapper(new System.Drawing.Pen(System.Drawing.Color.Red));
        }

        public IPen Red { get; }
    }
}

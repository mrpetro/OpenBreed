using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public interface IColorPalette
    {
        #region Public Properties

        MyColor[] Entries { get; }

        #endregion Public Properties
    }
}
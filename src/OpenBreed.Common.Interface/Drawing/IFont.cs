using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public interface IFont
    {
        #region Public Properties

        string Name { get; }
        float Size { get; }

        #endregion Public Properties
    }
}
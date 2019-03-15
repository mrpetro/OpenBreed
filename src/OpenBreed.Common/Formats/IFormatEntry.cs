using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public interface IFormatEntry
    {
        #region Public Properties

        string Name { get; }
        List<FormatParameter> Parameters { get; }

        #endregion Public Properties
    }
}
